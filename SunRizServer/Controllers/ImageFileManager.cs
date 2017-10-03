using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;
using System.Linq;
using System.IO;

namespace SunRizServer.Controllers
{
    public class ImageFileManager : BaseController, IUploadFileHandler
    {
        [RemotingMethod]
        public ImageFiles[] GetFiles(int parentid)
        {
            return (from m in db.ImageFiles
                    where m.ParentId == parentid
                    orderby m.IsFolder descending, m.Name
                    select m).ToArray();
        }

        [RemotingMethod]
        public ImageFiles AddFolder(string name,int parentid)
        {
            var data = new ImageFiles() {
                Name = name,
                IsFolder = true,
                ParentId = parentid,
            };
            this.db.Insert(data);
            return data;
        }

        [RemotingMethod]
        public ImageFiles AddFileByUpload(int parentid)
        {
            var data = new ImageFiles()
            {
                Name = Session["filename"].ToString(),
                IsFolder = false,
                FileName = Session["filepath"].ToString(),
                ParentId = parentid,
            };
            this.db.Insert(data);
            return data;
        }
        [RemotingMethod]
        public void ChangeName(string name , int id)
        {
            var data = db.ImageFiles.FirstOrDefault(m => m.id == id);
            data.Name = name;
            db.Update(data);
        }
        [RemotingMethod]
        public string GetNewFileName()
        {
            return Guid.NewGuid().ToString("N");
        }
        [RemotingMethod]
        public void DeleteFile(int id)
        {
            this.db.Delete( this.db.ImageFiles.Where(m=>m.id == id) );
        }
        FileStream _fs;
        string _filepath;
        public override IUploadFileHandler OnBeginUploadFile(string fileName, string state, int fileSize, int offset)
        {
            Session["filename"] = Path.GetFileName(fileName);
           
            _filepath = state + Path.GetExtension(fileName);
            Session["filepath"] = _filepath;

            if (System.IO.Directory.Exists(WebRoot + "ImageFiles") == false)
                System.IO.Directory.CreateDirectory(WebRoot + "ImageFiles");

            _fs = File.Create(WebRoot + "ImageFiles/" + _filepath);
            return this;
        }

        public void OnGettingFileData(byte[] data)
        {
            _fs.Write(data, 0, data.Length);
        }

        public void OnUploadFileCompleted()
        {
            _fs.Close();
            _fs.Dispose();
        }

        public void OnUploadFileError()
        {
            _fs.Dispose();
            File.Delete(WebRoot + "ImageFiles/" + _filepath);
        }
    }
}
