using EJClient.Forms.InterfaceCenter.Nodes;
using EJClient.TreeNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EJClient.Forms.InterfaceCenter
{
    class DllClassLoader
    {
        public static ClassNode GetClass(string filepath,string classFullName)
        {
            ClassNode result = null;
            XmlDocument xmldoc = new System.Xml.XmlDocument();
            xmldoc.Load(filepath);
            XmlNodeList memberNodes = xmldoc.SelectNodes("//member");

            foreach (XmlElement node in memberNodes)
            {
                string nameText = node.GetAttribute("name");
                string fullname = nameText.Substring(nameText.IndexOf(":") + 1);
                if (fullname.Contains("("))
                    fullname = fullname.Substring(0, fullname.IndexOf("("));

                string name = fullname;
                try
                {
                    name = fullname.Substring(fullname.LastIndexOf(".") + 1);
                }
                catch
                {
                }
                if (name == "#ctor")
                    continue;

                string comment = null;
                try
                {
                    comment = node.SelectSingleNode("summary").InnerText;
                }
                catch
                {
                }

                comment = comment.Trim();
                fullname = fullname.Trim();

                if (nameText.StartsWith("T:"))
                {
                    if (result != null)
                        return result;
                    if (fullname == classFullName)
                    {
                        result = new ClassNode(fullname, comment, null);
                    }
                }
                else if (nameText.StartsWith("M:"))
                {
                    if ( result != null && result.FullName + "." + name == fullname)
                    {
                        //是这个class的成员
                        result.Members.Add(new ClassNodeMember()
                        {
                            Type = ClassNodeType.method,
                            Comment = comment,
                            Name = name,
                        });
                    }
                }
                else if (nameText.StartsWith("P:"))
                {
                    if (result != null && result.FullName + "." + name == fullname)
                    {
                        //是这个class的成员
                        result.Members.Add(new ClassNodeMember()
                        {
                            Type = ClassNodeType.property,
                            Comment = comment,
                            Name = name,
                        });
                    }
                }
            }
            return result;
        }
        public static System.Collections.ObjectModel.ObservableCollection<TreeNodeBase> GetAllClass( string filepath)
        {
            System.Collections.ObjectModel.ObservableCollection<TreeNodeBase> classNodes = new System.Collections.ObjectModel.ObservableCollection<TreeNodeBase>();
            List<TreeNode.TreeNodeBase> list = new List<TreeNode.TreeNodeBase>();

            #region 获取所有类信息
            XmlDocument xmldoc = new System.Xml.XmlDocument();
            xmldoc.Load(filepath);
            XmlNodeList memberNodes = xmldoc.SelectNodes("//member");

            foreach (XmlElement node in memberNodes)
            {
                string nameText = node.GetAttribute("name");
                string fullname = nameText.Substring(nameText.IndexOf(":") + 1);
                if (fullname.Contains("("))
                    fullname = fullname.Substring(0, fullname.IndexOf("("));

                string name = fullname;
                try
                {
                    name = fullname.Substring(fullname.LastIndexOf(".") + 1);
                }
                catch
                {
                }
                if (name == "#ctor")
                    continue;

                string comment = null;
                try
                {
                    comment = node.SelectSingleNode("summary").InnerText;
                }
                catch
                {
                }

                fullname = fullname.Trim();
                comment = comment.Trim();

                if (nameText.StartsWith("T:"))
                {
                    ClassNode classNode = new ClassNode(fullname, comment, null);
                    list.Add(classNode);
                }
                else if (nameText.StartsWith("M:"))
                {
                    ClassNode classNode = (ClassNode)list.LastOrDefault();
                    if (classNode != null && classNode.FullName + "." + name == fullname)
                    {
                        //是这个class的成员
                        classNode.Members.Add(new ClassNodeMember()
                        {
                            Type = ClassNodeType.method,
                            Comment = comment,
                            Name = name,
                        });
                    }
                }
                else if (nameText.StartsWith("P:"))
                {
                    ClassNode classNode = (ClassNode)list.LastOrDefault();
                    if (classNode != null && classNode.FullName + "." + name == fullname)
                    {
                        //是这个class的成员
                        classNode.Members.Add(new ClassNodeMember()
                        {
                            Type = ClassNodeType.property,
                            Comment = comment,
                            Name = name,
                        });
                    }
                }
            }
            #endregion

            #region 把类分层次放入classNodes

            foreach (ClassNode node in list)
            {
                string[] paths = node.FullName.Split('.');
                System.Collections.ObjectModel.ObservableCollection<TreeNodeBase> currentNodes = classNodes;
                TreeNode.TreeNodeBase currentParent = null;
                for (int i = 0; i < paths.Length; i++)
                {
                    string itemName = paths[i];
                    var nodeItem = currentNodes.FirstOrDefault(m => m.Name == itemName);
                    if (nodeItem == null)
                    {
                        if (i == paths.Length - 1)
                        {
                            nodeItem = node;
                            nodeItem.Parent = currentParent;
                        }
                        else
                        {
                            nodeItem = new NamespaceNode(itemName, currentParent);
                        }
                        currentNodes.Add(nodeItem);
                    }
                    currentNodes = nodeItem.Children;
                    currentParent = nodeItem;
                }
            }
            #endregion
            return classNodes;
        }
    }
}
