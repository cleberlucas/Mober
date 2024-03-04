using Appwrite.Services;
using MoberUser = Mobile.Models.MoberUser;

namespace Mobile.DataStore.Externals
{
    public class AppwriteService
    {
        private readonly Databases _databases;

        public Databases Databases { get => _databases; }

        public AppwriteService()
        {
            var client = new Appwrite.Client().SetEndpoint("https://cloud.appwrite.io/v1")
                .SetProject("65e3efbba4a80c041e65")
                .SetKey("dc6d5e6c8e7cc25d321c8af9881c089e8acdacff8740d970506075a3b0fcbe5e997fa95586b985cb9d5dd6955e28b62526f455cffc59a41e87bf7f0f607b48a366a206d5cfec3cda036e13bc5d212ba18d0972695cab1a17f79afd3cadd3b948979e465b94f67fc278bff73c6dceef5984040d5cd8ae4e77e7f56d42270315db");

            _databases = new Databases(client);
        }

        public async Task<List<MoberUser>> GetUsers()
        {
            var response = new List<MoberUser>();

            var documentResult = await _databases.ListDocuments(databaseId: "65e3f020676a26467588", collectionId: "65e3f02a2cb8b928b5f0");

            foreach (var document in documentResult.Documents)
            {
                MoberUser user = new MoberUser();
                user.Name = document.Data["name"].ToString();
                user.Phone = document.Data["phone"].ToString();
                user.Longitude = Convert.ToDouble(document.Data["longitude"]);
                user.Latitude = Convert.ToDouble(document.Data["latitude"]);
                user.Service = document.Data["service"].ToString();
                user.Servant = Convert.ToBoolean(document.Data["servant"]);
                user.Updated = Convert.ToDateTime(document.Data["updated"]);
                user.Rate = Convert.ToInt16(document.Data["rate"]);

                response.Add(user);
            }

            return response;
        }

        public async Task<MoberUser> GetUser(string userName)
        {
            var response = new MoberUser();

            try
            {
                var documentResult = await _databases.GetDocument(
                    databaseId: "65e3f020676a26467588",
                    collectionId: "65e3f02a2cb8b928b5f0",
                    documentId: userName
                );

                response.Name = documentResult.Data["name"].ToString();
                response.Phone = documentResult.Data["phone"].ToString();
                response.Longitude = Convert.ToDouble(documentResult.Data["longitude"]);
                response.Latitude = Convert.ToDouble(documentResult.Data["latitude"]);
                response.Service = documentResult.Data["service"].ToString();
                response.Servant = Convert.ToBoolean(documentResult.Data["servant"]);
                response.Updated = Convert.ToDateTime(documentResult.Data["updated"]);
                response.Rate = Convert.ToInt16(documentResult.Data["rate"]);

            }
            catch { }

            return response;
        }

        public async Task CreateUser(MoberUser user)
        {
            await _databases.CreateDocument(
                    databaseId: "65e3f020676a26467588",
                    collectionId: "65e3f02a2cb8b928b5f0",
                    documentId: user.Name,
                    data: user
                );
        }

        public async Task UpdateUser(MoberUser user)
        {
            await _databases.UpdateDocument(
                    databaseId: "65e3f020676a26467588",
                    collectionId: "65e3f02a2cb8b928b5f0",
                    documentId: user.Name,
                    data: user
                );
        }
    }

}
