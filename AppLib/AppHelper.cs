using AppLib;
using AppLib.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;


public class AppHelper
{
    
    static List<LoadHistory> Histories = new List<LoadHistory>();

    #region chinese
    private static int[] pyValue = new int[]
    {
    -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
    -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
    -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
    -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
    -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
    -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
    -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
    -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
    -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
    -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
    -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
    -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
    -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
    -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
    -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
    -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
    -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
    -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
    -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
    -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
    -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
    -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
    -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
    -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
    -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
    -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
    -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
    -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
    -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
    -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
    -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
    -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
    -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
    };

    private static string[] pyName = new string[]
    {
    "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
    "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
    "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
    "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
    "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
    "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
    "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
    "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
    "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
    "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
    "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
    "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
    "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
    "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
    "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
    "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
    "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
    "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
    "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
    "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
    "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
    "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
    "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
    "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
    "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
    "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
    "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
    "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
    "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
    "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
    "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
    "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
    "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
    };

    /// <summary>
    /// 把汉字转换成拼音(全拼)
    /// </summary>
    /// <param name="hzString">汉字字符串</param>
    /// <returns>转换后的拼音(全拼)字符串</returns>
    public static string GetPinYinStrFromChinese(string hzString)
    {
        // 匹配中文字符
        Regex regex = new Regex("^[\u4e00-\u9fa5]{1}");
        byte[] array = new byte[2];
        string pyString = "";
        int chrAsc = 0;
        int i1 = 0;
        int i2 = 0;
        char[] noWChar = hzString.ToCharArray();

        for (int j = 0; j < noWChar.Length; j++)
        {
            // 中文字符
            if (regex.IsMatch(noWChar[j].ToString()))
            {
                array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                i1 = (short)(array[0]);
                i2 = (short)(array[1]);
                chrAsc = i1 * 256 + i2 - 65536;
                if (chrAsc > 0 && chrAsc < 160)
                {
                    pyString += noWChar[j];
                }
                else
                {
                    // 修正部分文字
                    if (chrAsc == -9254) // 修正“圳”字
                        pyString += "Zhen";
                    else
                    {
                        for (int i = (pyValue.Length - 1); i >= 0; i--)
                        {
                            if (pyValue[i] <= chrAsc)
                            {
                                pyString += pyName[i];
                                break;
                            }
                        }
                    }
                }
            }
            // 非中文字符
            else
            {
                pyString += noWChar[j].ToString();
            }
        }
        return pyString;
    }
    #endregion


   
     
    internal static Type FindTypeByFullname(string fullname)
    {
        var findInHis = (from m in Histories
                             where  m.InstanceType.FullName == fullname
                             select m.InstanceType).FirstOrDefault();
        if (findInHis != null)
            return findInHis;

         System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
         foreach (System.Reflection.Assembly assembly in assemblies)
         {
             Type[] types = assembly.GetTypes();
             var result = types.FirstOrDefault(m => m.FullName == fullname);
             if (result != null)
             {
                 Histories.Add(new LoadHistory()
                 {
                     InterfaceType = null,
                     InstanceType = result
                 });
                 return result;
             }
         }
         return null;      
    }

    internal static Way.EntityDB.DBContext CreateLinqDataBase(string fullname)
    {
        if(fullname.IsNullOrEmpty())
            throw new Exception("DatabaseConfig为空");
        Type type = FindTypeByFullname(fullname);
        if (type == null)
            throw new Exception("找不到类型" + fullname);
        //ConnectionStringConfig conStrs = new ConnectionStringConfig(null);

        //ConstructorInfo[] cinfos = type.GetConstructors();
        //foreach (ConstructorInfo info in cinfos)
        //{
        //    ParameterInfo[] pinfos = info.GetParameters();
        //    if (pinfos.Length == 1 && pinfos[0].ParameterType.IsSubclassOf(typeof(DataSpace.Database)))
        //    {
        //        return (Way.EntityDB.DBContext)Activator.CreateInstance(type, new object[] { DataSpace.Database.CreateDatabase(conStrs[databaseConfig]) });
        //    }
        //}
        return (Way.EntityDB.DBContext)Activator.CreateInstance(type);
    }


    /// <summary>
    /// 获取制定对象制定的属性值
    /// </summary>
    /// <param name="source"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static object Eval(object source, string propertyName)
    {
        return source.GetType().GetProperty(propertyName).GetValue(source);
    }

    #region JS RSA 加密解密
    private static string BytesToHexString(byte[] input)
    {
        StringBuilder hexString = new StringBuilder(64);

        for (int i = 0; i < input.Length; i++)
        {
            hexString.Append(String.Format("{0:X2}", input[i]));
        }
        return hexString.ToString();
    }

    static byte[] HexStringToBytes(string hex)
    {
        if (hex.Length == 0)
        {
            return new byte[] { 0 };
        }

        if (hex.Length % 2 == 1)
        {
            hex = "0" + hex;
        }

        byte[] result = new byte[hex.Length / 2];

        for (int i = 0; i < hex.Length / 2; i++)
        {
            result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        return result;
    }
   
    /// <summary>
    /// 生成公钥、私钥
    /// </summary>
    /// <returns>公钥、私钥，公钥键"PUBLIC",私钥键"PRIVATE"</returns>
    public static Dictionary<string, string> CreateKeyPair()
    {
        Dictionary<string, string> keyPair = new Dictionary<string, string>();
       
        RSACryptoServiceProvider provider = new RSACryptoServiceProvider(1024);
        keyPair.Add("FILE", AppLib.RSA.ExportPublicKeyToPEMFormat(provider));
        keyPair.Add("PUBLIC", provider.ToXmlString(false));
        keyPair.Add("PRIVATE", provider.ToXmlString(true));
        return keyPair;
    }
    /// <summary>   
    /// RSA解密   
    /// </summary>   
    /// <param name="encryptData">经过Base64编码的密文</param>   
    /// <param name="privateKey">私钥</param>   
    /// <returns>RSA解密后的数据</returns>   
    public static string DecryptWithRSAKey(string encryptData, string privateKey)
    {
        const int MAXDECRYPTSIZE = 128;
        string decryptData = "";
        try
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(privateKey);
            byte[] bEncrypt = Convert.FromBase64String(encryptData);
            int length = bEncrypt.Length;
            int offset = 0;
            string cache;
            int i = 0;
            while (length - offset > 0)
            {
                if (length - offset > MAXDECRYPTSIZE)
                {
                    cache = Encoding.UTF8.GetString(provider.Decrypt(getSplit(bEncrypt, offset, MAXDECRYPTSIZE), false));
                }
                else
                {
                    cache = Encoding.UTF8.GetString(provider.Decrypt(getSplit(bEncrypt, offset, length - offset), false));
                }
                decryptData += cache;
                i++;
                offset = i * MAXDECRYPTSIZE;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
        return decryptData;
    }

    /// <summary>   
    /// 截取字节数组部分字节   
    /// </summary>   
    /// <param name="input"></param>   
    /// <param name="offset">起始偏移位</param>   
    /// <param name="length">截取长度</param>   
    /// <returns></returns>   
    private static byte[] getSplit(byte[] input, int offset, int length)
    {
        byte[] output = new byte[length];
        for (int i = offset; i < offset + length; i++)
        {
            output[i - offset] = input[i];
        }
        return output;
    }
    /// <summary>
    /// 解密页面上post上来的数据
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string RSADecryptFromPostValue(string content )
    {
        return RSADecryptFromPostValue(content, HttpContext.Current.Session);
    }
    /// <summary>
    /// 解密页面上post上来的数据
    /// </summary>
    public static string RSADecryptFromPostValue(string content, System.Web.SessionState.HttpSessionState session)
    {
        if (session["rsa_private_key"] == null)
            throw new Exception("可能您离开的时间太长，信息已丢失，请重新登录再试");

        var rsa_private_key = session["rsa_private_key"].ToString();

        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(rsa_private_key);

        byte[] result = rsa.Decrypt(HexStringToBytes(content), false);
        System.Text.ASCIIEncoding enc = new ASCIIEncoding();
        string str = enc.GetString(result);
        return str;
    }
   
   
    /// <summary>
    /// 在页面末端输出RSA加密方法
    /// </summary>
    /// <param name="page"></param>
    public static void OutputRSAMethod(Page page)
    {
        //string jsfolder = HttpContext.Current.Request.MapPath("/inc/rsajs");
        //System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
     
        //string fileContent = File.ReadAllText(jsfolder + "\\key.data", System.Text.Encoding.UTF8);
        //string[] keys = json.Deserialize<string[]>(fileContent);
        //var rsa_private_key = keys[0];
        //var rsa_PublicKeyExponent = keys[1];
        //var rsa_PublicKeyModulus = keys[2];
       
        if ( page.Session["rsa_private_key"] == null)
        {

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //私钥
            string rsa_private_key = rsa.ToXmlString(true);
            //把公钥适当转换，准备发往客户端
            RSAParameters parameter = rsa.ExportParameters(true);
            string publicKeyExponent = BytesToHexString(parameter.Exponent);
            string publicKeyModulus = BytesToHexString(parameter.Modulus);
             page.Session["rsa_private_key"] = rsa_private_key;
             page.Session["rsa_PublicKeyExponent"] = publicKeyExponent;
             page.Session["rsa_PublicKeyModulus"] = publicKeyModulus;
        }
        var rsa_PublicKeyExponent =  page.Session["rsa_PublicKeyExponent"].ToString();
        var rsa_PublicKeyModulus =  page.Session["rsa_PublicKeyModulus"].ToString();

        using (CLog log = new CLog(""))
        {
            log.Log(HttpContext.Current.Request.Browser.Browser);
            log.Log(GetClientIP());
            for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
                log.Log(HttpContext.Current.Request.Cookies[i].Name + "=" + HttpContext.Current.Request.Cookies[i].Value);
            log.LogJson(rsa_PublicKeyExponent);
            log.LogJson(rsa_PublicKeyModulus);
        }

        if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "__JS_RSA"))
            page.ClientScript.RegisterStartupScript(page.GetType(), "__JS_RSA",
                @"
    <script src=""/inc/RSAJS/BigInt.js"" type=""text/javascript""></script>
    <script src=""/inc/RSAJS/RSA.js"" type=""text/javascript""></script>
    <script src=""/inc/RSAJS/Barrett.js"" type=""text/javascript""></script>
<script type=""text/javascript"">
        function RSAEncrypt(value) {
            setMaxDigits(129);
            var key = new RSAKeyPair(""" + rsa_PublicKeyExponent + @""", """", """ + rsa_PublicKeyModulus + @""");
            value = encryptedString(key, value);
            return value;
        }
    </script>
");
    }
    #endregion


    /// <summary>
    /// 获取当前客户端IP
    /// </summary>
    /// <returns></returns>
    public static string GetClientIP()
    {
        string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(ip))
            ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        return ip;
    }
    /// <summary>
    /// 给页面引入invoker.js文件
    /// </summary>
    /// <param name="page"></param>
    public static void RegisterInvokerJS(Page page)
    {
        if (!page.ClientScript.IsClientScriptIncludeRegistered("__Invoker_JSFile"))
        {
            page.ClientScript.RegisterClientScriptInclude("__Invoker_JSFile",
           page.ClientScript.GetWebResourceUrl(typeof(AppHelper),
                                       "AppLib.js.Invoker.js"));
        }
    }
    /// <summary>
    /// 给页面引入jquery.js文件
    /// </summary>
    /// <param name="page"></param>
    public static void RegisterJquery(Page page)
    {
        if (!page.ClientScript.IsClientScriptIncludeRegistered("__jquery_JSFile"))
        {
            page.ClientScript.RegisterClientScriptInclude("__jquery_JSFile",
           page.ClientScript.GetWebResourceUrl(typeof(AppHelper),
                                       "AppLib.js.jquery-1.11.3.js"));
        }
    }
    /// <summary>
    /// 对字符串进行md5加密
    /// </summary>
    /// <param name="strText"></param>
    /// <returns></returns>
    public static string MD5Encrypt(string strText)
    {
        if (strText != null && strText == "")
            return "";
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] result = md5.ComputeHash(System.Text.Encoding.GetEncoding("gb2312").GetBytes(strText));
        return EncryptString(Convert.ToBase64String(result));
    }

    internal static string GetDatabaseLinqType(object container)
    {
        AppLib.Controls.IAutoDataBindControl control = container as AppLib.Controls.IAutoDataBindControl;
        if (control == null)
        {
            if (container is IKeyObject)
            {
                control = ((IKeyObject)container).AutoDataBindControl;
            }
            else
            {
                return null;
            }
        }


        return control.DatabaseConfig;

    }
    //internal static string GetDatabaseDllName(object container)
    //{
    //    AppLib.Controls.IAutoDataBindControl control = container as AppLib.Controls.IAutoDataBindControl;
    //    if (control == null)
    //    {
    //        if (container is IKeyObject)
    //        {
    //            control = ((IKeyObject)container).AutoDataBindControl;
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }
    //    string dllname;
    //    System.Web.UI.WebControls.WebControl webcontrol = control as System.Web.UI.WebControls.WebControl;
    //    IWebApplication iwebapp = null;
    //    if (webcontrol != null && webcontrol.Site != null)
    //        iwebapp = webcontrol.Site.GetService(typeof(IWebApplication)) as IWebApplication;

    //    ConnectionStringConfig conStrs = new ConnectionStringConfig(iwebapp);
    //    dllname = conStrs.GetDllName(control.DatabaseConfig);
    //    if (!string.IsNullOrEmpty(dllname))
    //        return dllname;
    //    dllname = conStrs[control.DatabaseConfig].ToLower();


    //    if (dllname.Contains("initial catalog="))
    //    {
    //        dllname = Regex.Match(dllname, @"initial catalog=(?<a>(\w)+)").Groups["a"].Value + "DataObjects";
    //    }
    //    else if (dllname.Contains("database="))
    //    {
    //        dllname = Regex.Match(dllname, @"database=(?<a>(\w)+)").Groups["a"].Value + "DataObjects";
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //    return dllname;
    //}

    class AssemblyCache
    {
        public string DatabaseConfig;
        public Assembly Assembly;
    }
    static List<AssemblyCache> AssemblyCaches = new List<AssemblyCache>();
    internal static Assembly GetDatabaseAssembly(AppLib.Controls.IAutoDataBindControl control)
    {
        var cache = AssemblyCaches.FirstOrDefault(m => m.DatabaseConfig == control.DatabaseConfig);
        if (cache != null)
            return cache.Assembly;


        Type type = FindTypeByFullname(control.DatabaseConfig);
        if (type == null)
            throw new Exception("找不到类型" + control.DatabaseConfig);
        Type contextBaseType = typeof(Way.EntityDB.DBContext);

        while (type.BaseType != contextBaseType)
        {
            type = type.BaseType;
        }
        AssemblyCaches.Add(new AssemblyCache() { 
        Assembly = type.Assembly,
        DatabaseConfig = control.DatabaseConfig,
        });
        return type.Assembly;
    }

    public static void Alert(Page page, string msg)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), Guid.NewGuid().ToString(), "<script language=\"javascript\">alert(\"" + msg.Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n") + "\");</script>\r\n");
    }

    public static string MakeStringSmall(string text)
    {
        text = Regex.Replace(text, "[\f\n\r\t\v]", "");
        text = Regex.Replace(text, " {2,}", " ");
        return Regex.Replace(text, ">[ ]{1}", ">");
    }

    internal static Type GetDataItemType(IAutoDataBindControl control, string tableName)
    {
        //找出当前数据库对应的对象
        Assembly assembly = GetDatabaseAssembly(control);
        Type dataType = null;
        Type baseType = typeof(Way.EntityDB.DataItem);
        Type[] types = assembly.GetTypes();
        foreach (Type t in types)
        {
            if (t.IsSubclassOf(baseType) && t.Name == tableName)
            {
                dataType = t;
                break;
            }
        }

        return dataType;
    }
    internal static Type GetLinqDataItemType(IAutoDataBindControl control, string tableName)
    {
        //找出当前数据库对应的对象
        Assembly assembly = GetDatabaseAssembly(control);
        Type dataType = null;
        Type baseType = typeof(Way.EntityDB.DataItem);
        Type[] types = assembly.GetTypes();
        foreach (Type t in types)
        {
            if (t.IsSubclassOf(baseType) && t.Name == tableName)
            {
                dataType = t;
                break;
            }
        }

        return dataType;
    }

    /// <summary>
    /// 查看指定接口分别被哪些类实现了
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string ViewInterfaceImpls<T>()
    {
        Type interfacetype = typeof(T);
        StringBuilder result = new StringBuilder();

        System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (System.Reflection.Assembly assembly in assemblies)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.GetInterface(interfacetype.FullName) != null)
                {
                    result.AppendLine(type.FullName);
                    //获取构造函数信息
                    System.Reflection.ConstructorInfo[] constructorinfos = type.GetConstructors();
                    foreach (System.Reflection.ConstructorInfo info in constructorinfos)
                    {
                        result.AppendLine("\t- " + info.ToString());
                    }
                }
            }
        }


       
        return result.ToString();
    }

    /// <summary>
    /// 查找实现了指定基类的类
    /// </summary>
    /// <typeparam name="T">基类</typeparam>
    /// <returns></returns>
    public static Type[] ViewImplBaseType<T>()
    {
        Type basetype = typeof(T);
        List<Type> result = new List<Type>();
          System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
          foreach (System.Reflection.Assembly assembly in assemblies)
          {
              Type[] types = assembly.GetTypes();
              foreach (Type type in types)
              {
                  if (type.IsSubclassOf(basetype))
                  {
                      result.Add(type);
                  }
              }
          }
        return result.ToArray();
    }

    /// <summary>
    /// 查看指定接口分别被哪些类实现了
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Type[] ViewInterfaceTypes<T>()
    {
        Type interfacetype = typeof(T);
        List<Type> result = new List<Type>();

          System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
          foreach (System.Reflection.Assembly assembly in assemblies)
          {
              Type[] types = assembly.GetTypes();
              foreach (Type type in types)
              {
                  if (type.IsAbstract == false && type.GetInterface(interfacetype.FullName) != null)
                  {
                      result.Add(type);
                  }
              }
          }

        
        return result.ToArray();
    }

    /// <summary>
    /// 创建一个指定接口类型的实例
    /// </summary>
    /// <typeparam name="T">接口类型</typeparam>
    /// <returns></returns>
    public static T CreateInstance<T>()
    {
        return CreateInstance<T>(null);
    }

    /// <summary>
    /// 创建一个指定接口类型的实例
    /// </summary>
    /// <typeparam name="T">接口类型</typeparam>
    /// <param name="parameters">构造函数的参数</param>
    /// <returns></returns>
    public static T CreateInstance<T>(object[] parameters)
    {
       
        Type interfacetype = typeof(T);
        //查找以往记录，如果创建过这种接口的实例，直接用当时的类型创建
        for (int i = 0; i < Histories.Count; i++)
        {
            LoadHistory history = Histories[i];
            if (history.InterfaceType == interfacetype)
            {
                try
                {
                    object obj = Activator.CreateInstance(history.InstanceType, parameters);
                    return (T)obj;
                }
                catch
                {
                }
            }
        }


        //查找所有所有类型，看哪个实现了指定的接口
          System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
          foreach (System.Reflection.Assembly assembly in assemblies)
          {
              Type[] types = assembly.GetTypes();
              foreach (Type type in types)
              {
                  if (type.GetInterface(interfacetype.FullName) != null)
                  {
                      try
                      {
                          T obj = (T)Activator.CreateInstance(type, parameters);
                          Histories.Add(new LoadHistory()
                          {
                              InterfaceType = interfacetype,
                              InstanceType = type
                          });
                          return obj;
                      }
                      catch
                      {
                      }
                  }
              }
          }
        return default(T);
    }

    #region GetControlsByTypes
    /// <summary>
    /// 获取page里指定类型的控件集合
    /// </summary>
    /// <param name="page"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static List<Control> GetControlsByTypes(Page page, Type[] types)
    {
        return GetControlsByTypes(page.GetType(), page, types);
        List<Control> controls = new List<Control>();
        findcontrol(page.Controls, controls, new List<Type>(types));
        return controls;
    }
    public static List<Control> GetControlsByTypes(Type pagetype, object page, Type[] types)
    {
        List<Control> controls = new List<Control>();
        System.Reflection.FieldInfo[] fields = pagetype.GetFields(System.Reflection.BindingFlags.NonPublic | BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        foreach (System.Reflection.FieldInfo field in fields)
        {
            foreach (Type compareType in types)
            {
                if (field.FieldType == compareType || field.FieldType.IsSubclassOf(compareType))
                {
                    controls.Add(field.GetValue(page) as Control);
                }
            }
        }
        return controls;
    }
    internal static List<Control> GetControlsByTypes(ControlCollection _controls, Type[] types)
    {
        List<Control> controls = new List<Control>();
        findcontrol(_controls, controls, new List<Type>(types));
        return controls;
    }
    private static void findcontrol(System.Web.UI.ControlCollection controls, List<Control> pc, List<Type> types)
    {
        for (int i = 0; i < controls.Count; i++)
        {
            Control control = controls[i];
            Type t = control.GetType();
            bool sameType = false;
            foreach (Type mytype in types)
            {
                if (t == mytype || t.IsSubclassOf(mytype))
                {
                    sameType = true;
                    break;
                }
            }
            if (sameType)
            {
                pc.Add(control);
                findcontrol(control.Controls, pc, types);
            }
            else
            {
                findcontrol(control.Controls, pc, types);
            }


        }
    }
    #endregion

    #region 加密字符串
    ///// <summary>
    ///// 
    ///// 注意:密钥必须为８位
    ///// </summary>

    /// <summary>
    /// 加密字符串
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string EncryptString(string text)
    {
        string key = "j8$6(v;k";
        byte[] byKey = null;
        byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        try
        {
            byKey = System.Text.Encoding.UTF8.GetBytes(key);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byKey = ms.ToArray();
            StringBuilder result = new StringBuilder();
            foreach (byte b in byKey)
            {
                string hex = Convert.ToString((int)b, 16).PadLeft(2, '0');
                result.Append(hex);
            }
            return result.ToString();
        }
        catch (System.Exception ex)
        {
            throw (ex);
        }
    }
    #endregion

    #region 解密字符串
    public static string DecryptString(string text)
    {
        return DecryptString(text, "j8$6(v;k");
    }
    /// <summary>
    /// 解密字符串
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string DecryptString(string text, string key)
    {
        byte[] byKey = null;
        byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        byte[] inputByteArray = new Byte[text.Length];

        try
        {
            byKey = System.Text.Encoding.UTF8.GetBytes(key);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = new byte[text.Length / 2];
            for (int i = 0; i < inputByteArray.Length; i++)
            {
                inputByteArray[i] = (byte)Convert.ToInt32(text.Substring(i * 2, 2), 16);
            }
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetString(ms.ToArray());
        }
        catch (System.Exception ex)
        {
            throw (ex);
        }
    }
    #endregion
}

#region LoadHistory
/// <summary>
/// 加载过的历史记录
/// </summary>
class LoadHistory
{
    public Type InterfaceType;
    public Type InstanceType;
}
#endregion

public static class MyExtensions
{
    /// <summary>
    /// 保存图片
    /// </summary>
    /// <param name="img"></param>
    /// <param name="filepath">路径</param>
    /// <param name="widthOrHeight">另存为宽度</param>
    /// <param name="format">格式</param>
    public static void SaveAs(this System.Drawing.Image img,string filepath, int widthOrHeight,System.Drawing.Imaging.ImageFormat format)
    {
        if (img.Size.Width <= img.Size.Height)
        {
            int width = widthOrHeight;
            int height = (img.Size.Height * widthOrHeight) / img.Size.Width;
            using (System.Drawing.Bitmap newbitmap = new System.Drawing.Bitmap(width, height))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(newbitmap))
                {
                    g.DrawImage(img, new Rectangle(0, 0, width, height), new Rectangle(0, 0, img.Size.Width, img.Size.Height), GraphicsUnit.Pixel);
                }
                newbitmap.Save(filepath, format);
            }
        }
        else
        {
            int height = widthOrHeight;
            int width = (widthOrHeight * img.Size.Width) / img.Size.Height;
            using (System.Drawing.Bitmap newbitmap = new System.Drawing.Bitmap(width, height))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(newbitmap))
                {
                    g.DrawImage(img, new Rectangle(0, 0, width, height), new Rectangle(0, 0, img.Size.Width, img.Size.Height), GraphicsUnit.Pixel);
                }
                newbitmap.Save(filepath, format);
            }
        }
    }

    /// <summary>
    ///  给字符串加*掩码
    /// </summary>
    /// <param name="str"></param>
    /// <param name="maskLeft">是否掩盖左边字符</param>
    /// <param name="maskCenter">是否掩盖中间字符</param>
    /// <param name="maskRight">是否掩盖右边字符</param>
    /// <returns></returns>
    public static string ToMaskString(this string str,bool maskLeft , bool maskCenter , bool maskRight)
    {
        if (string.IsNullOrEmpty(str))
        {
            return "";
        }

        if (str.Length <= 2)
        {
            return "*" + str[str.Length - 1];
        }
        if (str.Length == 3)
        {
            return "*" + str.Substring(1);
        }
        if (str.Length > 15)
        {
            return (maskLeft ? "***" : str.Substring(0, 4)) + (maskCenter ? "***" : str.Substring(4 ,str.Length - 8)) + (maskRight ? "***" : str.Substring(str.Length - 4));
        }
        if (str.Length == 11)
        {

            int masklen = 3;
            return (maskLeft ? "***" : str.Substring(0, masklen)) + (maskCenter ? "*****" : str.Substring(masklen, str.Length - masklen * 2)) + (maskRight ? "***" : str.Substring(str.Length - masklen));
        }
        if (str.Length <= 15)
        {
            int masklen = str.Length / 3;
             return (maskLeft ? "***" : str.Substring(0, masklen)) + (maskCenter ? "***" : str.Substring(masklen ,str.Length - masklen*2)) + (maskRight ? "***" : str.Substring(str.Length - masklen));
        }
        return "***";
    }
    /// <summary>
    /// 取最后一个字符，前面以两个星号代替
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetRightChar(this string str)
    {
        if (str.IsNullOrEmpty())
            return "";
        return "**" + str.Substring(str.Length - 1);
    }

    public static System.Web.UI.WebControls.TableCell NextCell(this System.Web.UI.WebControls.TableCell cell)
    {
        try
        {
            return ((System.Web.UI.WebControls.TableCell)cell.Parent.Controls[cell.Parent.Controls.IndexOf(cell) + 1]);
        }
        catch
        {
        }
        return null;
    }

    /// <summary>
    /// 给字符串加*掩码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToMaskString(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return "";
        }
        if (str.Length <= 3)
        {
            return str.GetRightChar();
        }

        if (str.Length > 15)
        {
            return str.Substring(0, 4) + "***" + str.Substring(str.Length - 4);
        }
        if (str.Length <= 13)
        {
            int masklen = str.Length / 3;
            return str.Substring(0, masklen) + "****" + str.Substring(str.Length - masklen);
        }
        else
        {
            return str.Substring(0, 4) + "******" + str.Substring(str.Length - 2);
        }
    }
    public static string ToMaskString(this string str, bool doMask)
    {
        if (doMask)
            return ToMaskString(str);
        return str;
    }
    /// <summary>
    /// 四个数字加一个空格显示
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static string FormatNumberString(this string number)
    {
        if (number.IsNullOrEmpty())
            return "";

        int count = number.Length / 4;
        if (number.Length % 4 != 0)
            count++;
        string[] arr = new string[count];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = number.Substring(i * 4, Math.Min(4, number.Length - i * 4));
        }
        return arr.ToSplitString(" ");
    }

    /// <summary>
    /// 功能和ToString一样，null会返回""
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToSafeString(this object obj)
    {
        if (obj == null)
            return "";
        return obj.ToString();
    }
    /// <summary>
    /// 功能和ToString一样，null或者空字符，会返回noneString
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="noneString"></param>
    /// <returns></returns>
    public static string ToSafeString(this object obj,string noneString)
    {
        if (obj == null)
            return noneString;
        string result = obj.ToString();
        if (result.Length == 0)
            return noneString;
        return result;
    }
    /// <summary>
    /// 转换为时间格式
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this string str)
    {
        if (string.IsNullOrEmpty(str.Trim()))
        {
            return DateTime.MinValue;
        }
        return Convert.ToDateTime(str);
    }
    /// <summary>
    /// 每个对象输出一行字符
    /// </summary>
    /// <param name="arrs"></param>
    /// <returns></returns>
    public static string ToEachString(this Array arrs)
    {
        StringBuilder result = new StringBuilder();
        foreach (object str in arrs)
        {
            result.AppendLine(str.ToString());
        }
        return result.ToString();
    }
    /// <summary>
    /// 字符串转整型，空字符串返回0
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ToInt(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return 0;
        return Convert.ToInt32(str);
    }
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }
    public static double ToDouble(this object str)
    {
        if (str == null)
            return 0;
        try
        {
            return Convert.ToDouble(str);
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format(str + " ToDouble失败"));
        }
    }
    public static double ToDouble(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return 0;
        try
        {
            return Convert.ToDouble(str);
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format(str + " ToDouble失败"));
        }
    }

    /// <summary>
    /// 取小数点后四位，不四舍五入，后面的舍去
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static decimal ToRound4Decimal(this decimal number)
    {
        string[] arr = number.ToString().Split('.');
        if (arr.Length > 1)
        {
            return Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(4, arr[1].Length)));
        }
        else
        {
            return number;
        }
    }
    /// <summary>
    /// 取小数点后四位，不四舍五入，后面的舍去
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static decimal ToRound4Decimal(this double number)
    {
        string[] arr = number.ToString().Split('.');
        if (arr.Length > 1)
        {
            return Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(4, arr[1].Length)));
        }
        else
        {
            return Convert.ToDecimal( number);
        }
    }
    /// <summary>
    /// 去小数点后两位，有三位小数一律进1
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static decimal To2DecimalBigger(this decimal number)
    {
        string[] arr = number.ToString().Split('.');
        if (arr.Length > 1)
        {
            var result = Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(2, arr[1].Length)));
            if (result < number)
                result += 0.01m;
            return result;
        }
        else
        {
            return number;
        }
    }

    /// <summary>
    /// 去小数点后两位，有三位小数一律进1
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static decimal To2DecimalBigger(this decimal? number)
    {
        string[] arr = number.ToString().Split('.');
        if (arr.Length > 1)
        {
            var result = Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(2, arr[1].Length)));
            if (result < number)
                result += 0.01m;
            return result;
        }
        else
        {
            return number.GetValueOrDefault();
        }
    }

    /// <summary>
    /// 取小数点后2位，不四舍五入，后面的舍去
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static decimal ToRound2Decimal(this decimal number)
    {
        string[] arr = number.ToString().Split('.');
        if (arr.Length > 1)
        {
            return Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(2, arr[1].Length)));
        }
        else
        {
            return number;
        }
    }
    /// <summary>
    /// 取小数点后2位，不四舍五入，后面的舍去
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static decimal ToRound2Decimal(this decimal? number)
    {
        string[] arr = number.ToString().Split('.');
        if (arr.Length > 1)
        {
            return Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(2, arr[1].Length)));
        }
        else
        {
            return number.GetValueOrDefault();
        }
    }
    /// <summary>
    /// 取小数点后2位，不四舍五入，后面的舍去
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static decimal ToRound2Decimal(this double number)
    {
        string[] arr = number.ToString().Split('.');
        if (arr.Length > 1)
        {
            return Convert.ToDecimal(arr[0] + "." + arr[1].Substring(0, Math.Min(2, arr[1].Length)));
        }
        else
        {
            return Convert.ToDecimal(number);
        }
    }
    public static decimal ToDecimal(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return 0;
        return Convert.ToDecimal(str);
    }
   
    /// <summary>
    /// object输出money字符串，0输出空
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string NoZeroToMoney(this object value)
    {
        try{
            double d = Convert.ToDouble(value);
            if (d == 0)
                return "";
            string v = Math.Round(d, 2).ToString("n");
        if (v.Length == 0)
            return "";
        return v;
        }
        catch{
            return "";
        }
    }

    public static string ToMoney(this double value)
    {
        string v = Math.Round(Convert.ToDouble(value), 2).ToString("n");
        if (v.Length == 0)
            return "0";

        return v;
    }
    public static string ToMoney(this decimal value)
    {
        string v = Math.Round(Convert.ToDouble(value), 2).ToString("n");
        if (v.Length == 0)
            return "0";
        return v;
    }
    public static string ToMoney(this decimal? value)
    {
        string v = Math.Round(Convert.ToDouble(value.GetValueOrDefault()), 2).ToString("n");
        if (v.Length == 0)
            return "0";
        return v;
    }

    

    /// <summary>
    /// 每个对象逗号隔开
    /// </summary>
    /// <param name="arrs"></param>
    /// <returns></returns>
    public static string ToSplitString(this Array arrs)
    {
        return arrs.ToSplitString(",");
    }
    /// <summary>
    /// 用制定字符串联数组
    /// </summary>
    /// <param name="arrs"></param>
    /// <param name="splitchar">间隔字符</param>
    /// <returns></returns>
    public static string ToSplitString(this Array arrs,string splitchar)
    {
        StringBuilder result = new StringBuilder();
        foreach (object str in arrs)
        {
            if (result.Length > 0)
                result.Append(splitchar);
            result.Append(str.ToString().Trim());
        }
        return result.ToString();
    }
    public static string ToSplitString(this Array arrs, string splitchar,string itemFormat)
    {
        StringBuilder result = new StringBuilder();
        foreach (object str in arrs)
        {
            if (result.Length > 0)
                result.Append(splitchar);
            result.Append(string.Format(itemFormat , str));
        }
        return result.ToString();
    }
    //public static string ToString(this int? value)
    //{
    //    if (value == null)
    //        return "";
    //    else
    //        return value.GetValueOrDefault().ToString();
    //}
    //public static string ToString(this decimal? value)
    //{
    //    if (value == null)
    //        return "";
    //    else
    //        return value.GetValueOrDefault().ToString();
    //}
    //public static string ToString(this bool? value)
    //{
    //    if (value == null)
    //        return "";
    //    else
    //        return value.GetValueOrDefault().ToString();
    //}
    //public static string ToString(this double? value)
    //{
    //    if (value == null)
    //        return "";
    //    else
    //        return value.GetValueOrDefault().ToString();
    //}
}



