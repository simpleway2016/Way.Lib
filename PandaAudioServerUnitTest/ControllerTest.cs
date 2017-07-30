using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PandaAudioServer.Services;
using SysDB;
using PandaAudioServer.ActionCaptures;

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
    }
}
