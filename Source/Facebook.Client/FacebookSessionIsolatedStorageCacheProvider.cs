﻿//-----------------------------------------------------------------------
// <copyright file="FacebookSessionIsolatedStorageCacheProvider.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-winclient-sdk</website>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Facebook.Client
{
    public class FacebookSessionIsolatedStorageCacheProvider : FacebookSessionCacheProvider
    {
        private const string fileName = "FACEBOOK_SESSION";

        public override FacebookSession GetSessionData()
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            if (!store.FileExists(fileName))
            {
                return null;
            }

            FacebookSession data;
            var serializer = new XmlSerializer(typeof(FacebookSession));
            using (var stream = store.OpenFile(fileName, System.IO.FileMode.Open))
            {
                data = serializer.Deserialize(stream) as FacebookSession;
            }
            return data;
        }

        public override void SaveSessionData(FacebookSession data)
        {
            var serializer = new XmlSerializer(typeof(FacebookSession));
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            using (var stream = store.OpenFile(fileName, FileMode.Create))
            {
                serializer.Serialize(stream, data);
            }
        }

        public override void DeleteSessionData()
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            if (store.FileExists(fileName))
            {
                store.DeleteFile(fileName);
            }
        }
    }
}
