using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.HtmlUtil
{
    /// <summary>
    /// html解析器
    /// </summary>
    public class HtmlParser
    {
        internal static char[] KCHAR = new char[] { ' ', '\t', '\r', '\n' };
        public List<HtmlNode> Nodes
        {
            get;
            set;
        }
        public HtmlParser()
        {
            
        }
        /// <summary>
        /// 解析文本流
        /// </summary>
        /// <param name="stream"></param>
        public void Parse(StreamReader stream)
        {
            this.Nodes = new List<HtmlNode>();
            while (!stream.EndOfStream)
            {
                char c = (char)stream.Read();
                if(c == '<')
                {
                    HtmlNode node = new HtmlNode(null);
                    node.Parse(stream);
                    if (!string.IsNullOrEmpty(node.Name))
                    {
                        this.Nodes.Add(node);
                    }
                    
                }
            }
        }
    }

    /// <summary>
    /// html节点
    /// </summary>
    public class HtmlNode
    {
        static string[] SINGLENODENAMES = new string[] { "meta", "br" , "%@" };
        internal bool _ParseFinished = false;
        public HtmlNode Parent
        {
            get;
            private set;
        }
        public List<HtmlNode> Nodes
        {
            get;
            private set;
        }
        public List<HtmlNodeAttribute> Attributes
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            protected set;
        }
    
        internal HtmlNode(HtmlNode parent)
        {
            this.Parent = parent;
        }

        public virtual string getInnerHtml()
        {
            StringBuilder str = new StringBuilder();
            foreach( var node in Nodes )
            {
                str.AppendLine(node.ToString());
                if (node is HtmlTextBlock)
                    continue;
                string content = node.getInnerHtml();
                if(content.Length > 0)
                    str.AppendLine(content);
                if(String.Equals( node.Name , "input" , StringComparison.CurrentCultureIgnoreCase) == false)
                    str.AppendLine($"</{node.Name}>");
            }
            while( str.Length > 0 && ( str[str.Length - 1] == '\n' || str[str.Length - 1] == '\r' || str[str.Length - 1] == ' ' || str[str.Length - 1] == '\t'))
            {
                str.Remove(str.Length - 1, 1);
            }
            while (str.Length > 0 && (str[0] == '\n' || str[0] == '\r' || str[0] == ' ' || str[0] == '\t'))
            {
                str.Remove(0, 1);
            }
            return str.ToString();
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append('<');
            str.Append(Name);
            if(this.Attributes.Count > 0)
            {
                foreach( var att in Attributes )
                {
                    str.Append(' ');
                    str.Append(att.ToString());
                }
            }
            str.Append('>');
            return str.ToString();
        }

        /// <summary>
        /// 解析文本流
        /// </summary>
        /// <param name="stream"></param>
        internal void Parse(StreamReader stream)
        {
            this.Nodes = new List<HtmlNode>();
            this.Attributes = new List<HtmlNodeAttribute>();
            StringBuilder nameStr = new StringBuilder();
            char lastChar = '\0';
            bool hasInnerContent = true;

            while (!stream.EndOfStream && !_ParseFinished)
            {
                char c = (char)stream.Peek();
                if (HtmlParser.KCHAR.Contains(c))
                {
                    stream.Read();
                    lastChar = c;
                    if (nameStr.Length > 0)
                    {
                        HtmlNodeAttribute attr = new HtmlNodeAttribute(this);
                        attr.Parse(stream);
                        if (attr.Name.Length > 0)
                            this.Attributes.Add(attr);
                    }
                }
                else if(c == '>')
                {
                    stream.Read();
                    if (lastChar == '/' || String.Equals(nameStr.ToString(), "input" , StringComparison.CurrentCultureIgnoreCase))
                    {
                        hasInnerContent = false;
                    }
                    break;
                }
                else if (c == '/')
                {
                    if (nameStr.Length == 0)
                    {
                        break;
                    }
                    else
                    {
                        stream.Read();
                        lastChar = c;
                    }
                }
                else
                {
                    lastChar = c;
                    if (this.Attributes.Count > 0)
                    {
                        HtmlNodeAttribute attr = new HtmlNodeAttribute(this);
                        attr.Parse(stream);
                        if( attr.Name.Length > 0 )
                            this.Attributes.Add(attr);
                    }
                    else
                    {
                        stream.Read();
                        nameStr.Append(c);
                    }
                }
            }

            this.Name = nameStr.ToString();
           
            if ( this.Name.Length == 0 || SINGLENODENAMES.Any(m=>m == this.Name.ToLower())|| hasInnerContent == false || this.Name.StartsWith("!"))
                return;

            parseInnerContent(stream);
        }

        void parseInnerContent(StreamReader stream)
        {
            string nodename = this.Name.ToLower();
            List<Pair> pairs = null;
            if(nodename == "style" || nodename == "script")
            {
                pairs = new List<Pair>();
            }
            HtmlTextBlock textNode = new HtmlUtil.HtmlTextBlock(this);
            _runagain:
            while (!stream.EndOfStream && !_ParseFinished)
            {
                char c = (char)stream.Read();
                if(pairs != null && pairs.Count > 0)
                {
                    if(pairs.Last().IsMyEndChar(c) && textNode.Text.Last() != '\\')
                    {
                        textNode.Text += c;
                        pairs.RemoveAt(pairs.Count - 1);
                    }
                    else
                    {
                        textNode.Text += c;
                        if( pairs.Last()._char == '{' || pairs.Last()._char == '(')
                        {
                            if (textNode.Text.EndsWith("/*"))
                            {
                                parseComment1(stream, textNode);
                            }
                            else if (textNode.Text.EndsWith("//"))
                            {
                                parseComment2(stream, textNode);
                            }
                        }
                    }
                }
                else if(pairs != null && Pair.IsPair(c) )
                {
                    pairs.Add(new Pair(c));
                    textNode.Text += c;
                }
                else if (c == '<')
                {
                    if (textNode.Text.Length > 0)
                    {
                        this.Nodes.Add(textNode);
                        textNode = new HtmlUtil.HtmlTextBlock(this);
                    }

                    HtmlNode node = new HtmlNode(this);
                    node.Parse(stream);
                    if (node.Name.Length == 0)
                    {
                        c = (char)stream.Peek();
                        if (c == '/')
                        {
                            //结束标签
                            stream.Read();
                            string endTagName = "";
                            while (!stream.EndOfStream  )
                            {
                                c = (char)stream.Read();
                                if (c == '>')
                                {
                                    if (endTagName != this.Name)
                                    {
                                        HtmlNode parentNode = this.Parent;
                                        List<HtmlNode> myParentNodes = new List<HtmlNode>();
                                        while (parentNode != null)
                                        {
                                            if (parentNode.Name == endTagName)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                myParentNodes.Add(parentNode);
                                                parentNode = parentNode.Parent;
                                            }
                                        }
                                        if (parentNode != null)
                                        {
                                            parentNode._ParseFinished = true;
                                            foreach (var p in myParentNodes)
                                                p._ParseFinished = true;
                                        }
                                        else
                                        {
                                            //找不到与这个结束符匹配的开始符，所以当作内容了
                                            textNode.Text += "</" + endTagName + ">";
                                            goto _runagain;
                                        }
                                    }

                                    break;
                                }
                                else
                                    endTagName += c;
                            }
                                
                            break;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(node.Name))
                        {
                            this.Nodes.Add(node);
                        }
                    }
                }
                else if (HtmlParser.KCHAR.Contains(c))
                {
                    if(textNode.Text.Length > 0)
                    {
                        textNode.Text += c;
                    }
                }
                else
                {
                    textNode.Text += c;
                    if (textNode.Text.EndsWith("/*"))
                    {
                        parseComment1(stream, textNode);
                    }
                    else if (textNode.Text.EndsWith("//"))
                    {
                        parseComment2(stream, textNode);
                    }
                }
            }

            if(textNode.Text.Length > 0 )
            {
                this.Nodes.Add(textNode);
            }
        }

        void parseComment1(StreamReader stream,HtmlTextBlock textNode)
        {
            //注释忽略掉
            string str = "";
            while (!stream.EndOfStream)
            {
                char c = (char)stream.Read();
                str += c;
                if (str.EndsWith("*/"))
                {
                    textNode.Text = textNode.Text.Remove(textNode.Text.Length - 2, 2);
                    break;
                }
            }
        }
        void parseComment2(StreamReader stream, HtmlTextBlock textNode)
        {
            //注释忽略掉
            string str = "";
            while (!stream.EndOfStream)
            {
                char c = (char)stream.Read();
                str += c;
                //textNode.Text += c;
                if (c == (char)10 || c == (char)13)
                {
                    textNode.Text = textNode.Text.Remove(textNode.Text.Length - 2, 2);
                    break;
                }
            }
        }
        class Pair
        {
            internal char _char;
            public Pair( char c)
            {
                _char = c;
            }
            public bool IsMyEndChar(char c)
            {
                switch(_char)
                {
                    case '\'':
                        if (c == '\'')
                            return true;
                        break;
                    case '\"':
                        if (c == '\"')
                            return true;
                        break;
                    case '{':
                        if (c == '}')
                            return true;
                        break;
                    case '(':
                        if (c == ')')
                            return true;
                        break;
                }
                return false;
            }
            public static bool IsPair(char c)
            {
                if (c == '\'' || c == '\"' || c == '{' || c == '(')
                    return true;
                return false;
            }
        }
    }

    public class HtmlTextBlock : HtmlNode
    {
        public string Text
        {
            get;
            internal set;
        }
        public HtmlTextBlock(HtmlNode parent):base(parent)
        {
            this.Text = "";
        }
        public override string getInnerHtml()
        {
            return this.Text.ToString();
        }
        public override string ToString()
        {
            return this.Text.ToString();
        }
    }

    public class HtmlNodeAttribute
    {
        public HtmlNode Node
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            private set;
        }
        public string Value
        {
            get;
            private set;
        }
        internal HtmlNodeAttribute(HtmlNode node)
        {
            this.Node = node;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Value))
                return string.Format("{0}=\"{1}\"", this.Name, this.Value);
            else
                return this.Name;
        }
        internal void Parse(StreamReader stream)
        {
            StringBuilder nameStr = new StringBuilder();

            while (!stream.EndOfStream)
            {
                char c = (char)stream.Peek();
                if (HtmlParser.KCHAR.Contains(c))
                {
                    stream.Read();
                    if (nameStr!= null && nameStr.Length > 0)
                    {
                        this.Name = nameStr.ToString();
                        nameStr = null;
                    }
                }
                else if (c == '>' || c == '/')
                {
                    break;
                }
                else if (c == '=')
                {
                    stream.Read();
                    ParseValue(stream);
                    break;
                }
                else
                {
                    if (nameStr != null)
                    {
                        stream.Read();
                        nameStr.Append(c);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (nameStr != null)
            {
                this.Name = nameStr.ToString();
                nameStr = null;
            }
        }

        void ParseValue(StreamReader stream)
        {
            char lastchar = '\0';
            char endChar = '\0';
            StringBuilder valueStr = new StringBuilder();
            while (!stream.EndOfStream)
            {
                char c = (char)stream.Read();
                if (c == '\"' || c == '\'')
                {
                    if(endChar == '\0')
                    {
                        endChar = c;
                    }
                    else if(c == endChar && lastchar != '\\')
                    {
                        break;
                    }
                    else
                    {
                        lastchar = c;
                        valueStr.Append(c);
                    }
                }
                else if (HtmlParser.KCHAR.Contains(c))
                {
                    if(endChar!='\0')
                    {
                        lastchar = c;
                        valueStr.Append(c);
                    }
                }
                else
                {
                    lastchar = c;
                    valueStr.Append(c);
                }
            }

            this.Value = valueStr.ToString();
        }
    }

}
