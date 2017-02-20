调用方必须实现IPayResultListener和IPayConfig两个接口
调用方在Global里面，初始化，例子如下
		
		static Global()
        {
            //激活支付功能
            PayFactory.Enable(new TestConfig() , new TestResultListener());

        }

配置参数格式示例：

AlipayWebPay：
<?xml version="1.0" encoding="utf-8"?>
<alipayConfig>
  <pid>208332330344051</pid>
  <key>87i1vbdfafedofnvx24snd71jjz9v0k</key>
</alipayConfig>

AlipayBarcode、AlipayScanQRCode：
<?xml version="1.0" encoding="utf-8"?>
<alipayConfig>
  <appId>2016091000477416</appId>
  <pid>2088102174950261</pid>
  <serverUrl>https://openapi.alipaydev.com/gateway.do</serverUrl>
  <mapiUrl>https://mapi.alipaydev.com/gateway.do</mapiUrl>
  <monitorUrl>http://mcloudmonitor.com/gateway.do</monitorUrl>
  <alipay_public_key>6FTFY99uhpiqTcZ32oWpwIDAQAB</alipay_public_key>
  <merchant_private_key>mWZEpYxg/QD/Qx2gcX6W4Ua3Co8B1/iUEA==</merchant_private_key>
  <merchant_public_key>8PcifJb6MdJ0w/K+FZJMXmWZEpYxg/QD/Qx2gcGtCza/xElcHR3s2z</merchant_public_key>
</alipayConfig>


微信：
<?xml version="1.0" encoding="utf-8"?>
<wechatPayConfig>
  <AppID>wx23423424</AppID>
  <AppSecret>51c56b886b53e5d1d6</AppSecret>
  <MchID>13866767402</MchID>
  <Key>A47a6B74n08D2D5Aw00</Key>
 <SSLCERT_PATH>apiclient_cert.p12</SSLCERT_PATH>
</wechatPayConfig>