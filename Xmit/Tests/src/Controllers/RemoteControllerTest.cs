using NUnit.Framework;
using System;
using System.Collections.Generic;
using OracleFirewall.Controllers;
using OracleFirewall.DTOs;
using OracleFirewall.Interfaces;
using Newtonsoft.Json;
namespace Tests {
    [TestFixture]
    public class RemoteControllerTest {
        public RemoteController SetupObject() {
            var rc = new RemoteController();
            rc.Request = GlobalHelper.GetRequestObject(rc);
            var uri = new System.Uri("http://www.contoso.com/?l=true&c=12&d=x");
            rc.Request.RequestUri = uri;
            return rc;
        }

        [Test]
        public void ProcessCvhFile_HappyPath() {
            var rc = SetupObject();
            var data = DataTestMock.GetData();
            var settings = new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var obj = JsonConvert.SerializeObject(data, Formatting.None, settings);
            var ret = rc.ProcessCvhFile(obj, new OracleControllerMock());
            Assert.IsTrue(ret != null);
        }
    }
}
