using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PandaAudioServer.Services;
using SysDB;
using PandaAudioServer.ActionCaptures;
using System.Security.Cryptography;

namespace PandaAudioServerUnitTest
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void EnumInserTest()
        {
            var db = new PandaAudioServer.PandaDB();
            try
            {
                db.BeginTransaction();

                UserEffect effect = new UserEffect();
                effect.FileName = Guid.NewGuid().ToString();
                effect.Type = UserEffect_TypeEnum.Project;
                db.Insert(effect);
            }
            catch
            {
                throw;
            }
            finally
            {
                db.RollbackTransaction();
            }
        }


        [TestMethod]
        public void EffectDeleteTest()
        {
            PandaAudioServer.PandaDB.RegisterActionCapture(new UserEffect_Capture());
            var db = new PandaAudioServer.PandaDB();
            try
            {
                db.BeginTransaction();

                UserEffect effect = new UserEffect();
                effect.FileName = Guid.NewGuid().ToString();
                effect.Type = UserEffect_TypeEnum.Project;
                db.Insert(effect);

                db.Delete(effect);
            }
            catch
            {
                throw;
            }
            finally
            {
                db.RollbackTransaction();
            }
        }

        [TestMethod]
        public void RSATest()
        {
            Way.Lib.RSA rsa = new Way.Lib.RSA();

            RSAParameters rsaparam = new RSAParameters();
            rsaparam.Exponent = rsa.Exponent;
            rsaparam.Modulus = rsa.Modulus;

            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.KeySize = 1024;
                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                RSA.ImportParameters(rsaparam);

                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                var c = RSA.Encrypt(new byte[] { 0x1 }, false);

                var result = rsa.Decrypt(c);
            }
        }
    }
}
