using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace API
{
    // ������ ������ ��� �������� �� ������
    [Serializable]
    public class ResourcePlayer
    {
        public bool isPlayerRegistration = false;
        public int currentIndexDialogPoint = 0;
        public int currentIndexDialog = 0;
        public List<ExerciseSave> exerciseSaves;
        public List<IngradientSave> ingradientSaves;
        public ItemFromTableSave currentItemFromTableSave;
        public List<ModelBoardSave> modelBoardSaves;
        public List<ItemFromTableSave> itemFromTableSaves;
        public List<MagnetSave> magnetSaves;
        public List<SaveTypeProduct> products;
    }

    [Serializable]
    // ��� � ���������� ������ � ������
    public class ResourceChangedPlayer
    {
        public Dictionary<string, string> changedResources;
    }

    [Serializable]
    // ��� ������
    public class LogPlayer
    {
        public string comment;
        public string player_name;
        public ResourceChangedPlayer resources_changed;
    }

    [Serializable]
    // ��� ��������
    public class LogShop
    {
        public string comment;
        public string player_name;
        public ResourceChangedShop resources_changed;
    }

    // ������ �������� ������ ��� �������� �� ������
    [Serializable]
    public class ResourceShop
    {
        public bool isShopRegistration = false;
        public List<ProductSave> productSaves;
    }

    [Serializable]
    // ��� � ���������� ������ � �������� ������
    public class ResourceChangedShop
    {
        public Dictionary<string, string> changedResources;
    }


    [Serializable]
    // ����� � ������ � ���������
    public class JSONPlayer
    {
        public string nameUser;
        public ResourcePlayer resources;
    }

    [Serializable]
    // ������� � ������ � ������ ��������� � ��� ���������
    public class JSONShop
    {
        public string nameShop;
        public ResourceShop resources;
    }

    [Serializable]
    // ������ ��� ������ �����
    public class JSONErrorLog
    {
        public string Error;
    }

    [Serializable]
    // ������ ��� ��������� ��������
    public class JSONError
    {
        public string Detail;
    }

    public class ClientHandler : MonoBehaviour
    {
        [SerializeField] private string UUID;
        public UnityEvent OnNotInternet;

        private void Start()
        {
            // ����������� ������ � ������� ��������, ���� ����� ��������� ��������, ������� null
            //ResourcePlayer resourcePlayer = new ResourcePlayer();
            //resourcePlayer.apple = 10;
            //RegistrationPlayer("Den4o", resourcePlayer);
            //RegistrationPlayer("Den4o", null);

            // �������� �������� ������ �� ������
            //ResourcePlayer resourcePlayer = new ResourcePlayer();
            //resourcePlayer.apple = 10;
            //SetResourcePlayer("Den4o", resourcePlayer);

            // ��������� �������� ������
            //ResourcePlayer resourcePlayerGet = await GetResourcePlayer("Den4o");

            // ��������� ������ ������� � �� ������� � ���������
            //List<JSONPlayer> listPlayer = await GetListPlayers();

            // �������� ������� ������ �� �������
            //Debug.Log(await HasPlayerInServer("Den4o"));

            // �������� ������ �� �����
            //DeletePlayer("Den4o");

            // �������� ����� � ������ ��� ���. ���������, ����������� ��� ������, ����������� � ���� � ���������� ������� � ���������
            //ResourceChangedPlayer resourceChangedPlayer = new ResourceChangedPlayer();
            //resourceChangedPlayer.add_apple = 10;
            //CreateLogPlayer("Den4o", "123", resourceChangedPlayer);

            // ��������� ���� ����� ������
            //List<LogPlayer> listPlayer = await GetListLogsPlayer("Den4o");
            //Debug.Log(listPlayer[0].player_name);

            // ����������� �������� ��� ������
            //ResourceShop resourceShop = new ResourceShop();
            //resourceShop.apple = 10;
            //RegistrationShop("Den4o", "Shop", resourceShop);

            // ��������� �������� � �������� ������
            //ResourceShop resourceShopGet = await GetResourceShopPlayer("Den4o", "Shop");
            //Debug.Log(resourceShopGet.apple);

            // �������� �������� �������� ������ �� ������
            //ResourceShop resourceShop = new ResourceShop();
            //resourceShop.apple = 10;
            //SetResourceShopPlayer("Den4o", "Shop", resourceShop);

            // �������� �������� � ������
            //DeleteShop("Den4o", "Shop");

            // �������� ������ ��������� � ������
            //List<JSONShop> listShop = await GetListShopPlayer("Den4o");
            //Debug.Log(listShop.First().name);

            // �������� ����� � �������� ������ ��� ���. ���������, ����������� ��� ������, ����������� � ���� � ���������� ������� � ���������
            //ResourceChangedShop resourceChangedShop = new ResourceChangedShop();
            //resourceChangedShop.add_apple = 10;
            //CreateLogShop("Den4o", "Shop", "123", resourceChangedShop);

            // ��������� ���� ����� �������� ������
            //GetListLogsShop("Den4o", "Shop");

            // ��������� ���� ����� � ����
            //GetLogsGame();
        }

        #region Player

        public async void RegistrationPlayer(string userName, ResourcePlayer resourcePlayer)
        {
            if (CheckInternetConnection("google.com") == false)
                return;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                FormUrlEncodedContent content;

                if (resourcePlayer != null)
                {
                    content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("name", userName),
                        new KeyValuePair<string, string>("resources", JsonConvert.SerializeObject(resourcePlayer))
                    });
                }
                else
                {
                    content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("name", userName),
                    });
                }

                request.Content = content;
                var response = await client.SendAsync(request);
                try
                {
                    response.EnsureSuccessStatusCode();
                    Debug.Log(await response.Content.ReadAsStringAsync());
                }
                catch
                {
                    Debug.Log($"�������� {userName} ��� ��������������� ��� �������������� ������");
                }
            }
        }

        public async Task<ResourcePlayer> GetResourcePlayer(string userName)
        {
            if (CheckInternetConnection("google.com") == false)
                return null;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{userName}/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL);
                try
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    JSONPlayer JSONPlayer = JsonConvert.DeserializeObject<JSONPlayer>(json);
                    if(JSONPlayer.resources == null)
                        Debug.LogWarning($"�������������� �� ���������� �������� � ������ {JSONPlayer.nameUser}, �������� ������ � ����������");
                    return JSONPlayer.resources;
                }
                catch
                {
                    Debug.Log($"����� {userName} ���������� �� �������. ������");
                }
            }
            return null;
        }

        public async Task<bool> HasPlayerInServer(string userName)
        {
            if (CheckInternetConnection("google.com") == false)
                return false;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{userName}/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL);
                try
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public async void SetResourcePlayer(string userName, ResourcePlayer resourcePlayer)
        {
            if (CheckInternetConnection("google.com") == false)
                return;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{userName}/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, URL);
                FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("resources",  JsonConvert.SerializeObject(resourcePlayer))
                });
                request.Content = content;
                var response = await client.SendAsync(request);

                string json = await response.Content.ReadAsStringAsync();
                JSONError jsonError = null;
                try
                {
                    jsonError = JsonConvert.DeserializeObject<JSONError>(json);
                }
                catch { }

                if (jsonError != null && jsonError.Detail == "No Player matches the given query.")
                {
                    Debug.Log($"����� {userName} ���������� �� �������. ������: No Player matches the given query.");
                    return;
                }

                Debug.Log(await response.Content.ReadAsStringAsync());
            }
        }

        public async void DeletePlayer(string userName)
        {
            if (CheckInternetConnection("google.com") == false)
                return;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{userName}/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, URL);
                var response = await client.SendAsync(request);

                string json = await response.Content.ReadAsStringAsync();
                JSONError jsonError = null;
                try
                {
                    jsonError = JsonConvert.DeserializeObject<JSONError>(json);
                }
                catch { }

                if (jsonError != null && jsonError.Detail == "No Player matches the given query.")
                {
                    Debug.Log($"����� {userName} ���������� �� �������. ������: No Player matches the given query.");
                    return;
                }

                Debug.Log($"����� {userName} ������ � �������");
            }
        }

        public async Task<List<JSONPlayer>> GetListPlayers()
        {
            if (CheckInternetConnection("google.com") == false)
                return null;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL);
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                List<JSONPlayer> listPlayers = JsonConvert.DeserializeObject<List<JSONPlayer>>(json);

                for (int i = 0; i < listPlayers.Count; i++)
                {
                    if (listPlayers[i].resources == null)
                        Debug.LogWarning($"�������������� �� ���������� �������� � ������ {listPlayers[i].nameUser}, �������� ������ � ����������");
                }

                Debug.Log(await response.Content.ReadAsStringAsync());
                return listPlayers;
            }
            return null;
        }
        #endregion
        #region LogPlayer
        public async void CreateLogPlayer(string userName, string comment, ResourceChangedPlayer resourceChangedPlayer)
        {
            if (CheckInternetConnection("google.com") == false)
                return;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/logs/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("player_name", $"{userName}"),
                    new KeyValuePair<string, string>("comment", comment),
                    new KeyValuePair<string, string>("resources_changed", JsonConvert.SerializeObject(resourceChangedPlayer))
                });
                request.Content = content;
                HttpResponseMessage response = await client.SendAsync(request);

                string json = await response.Content.ReadAsStringAsync();
                JSONErrorLog jsonError = null;
                try
                {
                    jsonError = JsonConvert.DeserializeObject<JSONErrorLog>(json);
                }
                catch { }

                if (jsonError != null && jsonError.Error == "Not existing Player")
                {
                    Debug.Log($"����� {userName} ���������� �� �������. ������: Not existing Player");
                    return;
                }

                Debug.Log(await response.Content.ReadAsStringAsync());
            }
        }


        public async Task<List<LogPlayer>> GetListLogsPlayer(string userName)
        {
            if (CheckInternetConnection("google.com") == false)
                return null;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{userName}/logs/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL);
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                List<LogPlayer> listLogsPlayer = null;
                try
                {
                    listLogsPlayer = JsonConvert.DeserializeObject<List<LogPlayer>>(json);
                }
                catch { }

                Debug.Log(await response.Content.ReadAsStringAsync());
                return listLogsPlayer;
            }
            return null;
        }
        #endregion
        #region Shop
        public async void RegistrationShop(string userName, string nameShop, ResourceShop resourceShop)
        {
            if (CheckInternetConnection("google.com") == false)
                return;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{userName}/shops/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("name", nameShop),
                    new KeyValuePair<string, string>("resources", JsonConvert.SerializeObject(resourceShop))
                });
                request.Content = content;
                var response = await client.SendAsync(request);
                try
                {
                    response.EnsureSuccessStatusCode();
                    Debug.Log(await response.Content.ReadAsStringAsync());
                }
                catch
                {
                    Debug.Log($"������� {nameShop} ��� ��������������� ");
                }
            }
        }

        public async Task<ResourceShop> GetResourceShopPlayer(string userName, string nameShop)
        {
            if (CheckInternetConnection("google.com") == false)
                return null;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{userName}/shops/{nameShop}/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL);

                try
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    JSONShop JSONShop = JsonConvert.DeserializeObject<JSONShop>(json);
                    if (JSONShop.resources == null)
                        Debug.LogWarning($"�������������� �� ���������� �������� � �������� {nameShop} � ������ {userName}, �������� ������ � ����������");

                    return JSONShop.resources;
                }
                catch
                {
                    Debug.Log($"������� {nameShop} � ������ {userName} ���������� �� �������. ������");
                }
            }
            return null;
        }

        public async void SetResourceShopPlayer(string userName, string nameShop, ResourceShop resourceShop)
        {
            if (CheckInternetConnection("google.com") == false)
                return;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{userName}/shops/{nameShop}/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, URL);
                FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("resources", JsonConvert.SerializeObject(resourceShop))
                });
                request.Content = content;
                var response = await client.SendAsync(request);

                string json = await response.Content.ReadAsStringAsync();
                JSONError jsonError = null;
                try
                {
                    jsonError = JsonConvert.DeserializeObject<JSONError>(json);
                }
                catch { }

                if (jsonError != null && jsonError.Detail == "No Shop matches the given query.")
                {
                    Debug.Log($"������� {nameShop} � ������ {userName} ���������� �� �������. ������: No Shop matches the given query.");
                    return;
                }

                Debug.Log(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<JSONShop>> GetListShopPlayer(string userName)
        {
            if (CheckInternetConnection("google.com") == false)
                return null;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{userName}/shops/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL);
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                List<JSONShop> listShop = JsonConvert.DeserializeObject<List<JSONShop>>(json);

                for (int i = 0; i < listShop.Count; i++)
                {
                    if (listShop[i].resources == null)
                        Debug.LogWarning($"�������������� �� ���������� �������� � �������� {listShop[i].nameShop} � ������ {userName}, �������� ������ � ����������");
                }

                Debug.Log(await response.Content.ReadAsStringAsync());
                return listShop;
            }
            return null;
        }

        public async void DeleteShop(string userName, string nameShop)
        {
            if (CheckInternetConnection("google.com") == false)
                return;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{userName}/shops/{nameShop}/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, URL);
                var response = await client.SendAsync(request);

                string json = await response.Content.ReadAsStringAsync();
                JSONError jsonError = null;
                try
                {
                    jsonError = JsonConvert.DeserializeObject<JSONError>(json);
                }
                catch { }

                if (jsonError != null && jsonError.Detail == "No Shop matches the given query.")
                {
                    Debug.Log($"������� {nameShop} � ������ {userName} ���������� �� �������. ������: No Shop matches the given query.");
                    return;
                }

                Debug.Log($"������� {nameShop} � ������ {userName} ������ � �������");
            }
        }
        #endregion
        #region LogShop

        public async void CreateLogShop(string userName, string shopName, string comment, ResourceChangedShop resourceChangedShop)
        {
            if (CheckInternetConnection("google.com") == false)
                return;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/logs/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("player_name", userName),
                    new KeyValuePair<string, string>("shop_name", shopName),
                    new KeyValuePair<string, string>("comment", comment),
                    new KeyValuePair<string, string>("resources_changed", JsonConvert.SerializeObject(resourceChangedShop))
                });
                request.Content = content;
                var response = await client.SendAsync(request);

                string json = await response.Content.ReadAsStringAsync();
                JSONErrorLog jsonError = null;
                try
                {
                    jsonError = JsonConvert.DeserializeObject<JSONErrorLog>(json);
                }
                catch { }

                if (jsonError != null && jsonError.Error == "Not existing Shop")
                {
                    Debug.Log($"����� {userName} ���������� �� �������. ������: Not existing Shop");
                    return;
                }

                Debug.Log(await response.Content.ReadAsStringAsync()); 
            }
        }

        public async Task<List<LogShop>> GetListLogsShop(string userName, string shopName)
        {
            if (CheckInternetConnection("google.com") == false)
                return null;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/players/{userName}/shops/{shopName}/logs/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL);
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                List<LogShop> listLogsShop = null;
                try
                {
                    listLogsShop = JsonConvert.DeserializeObject<List<LogShop>>(json);
                }
                catch { }

                Debug.Log(await response.Content.ReadAsStringAsync());
                return listLogsShop;
            }
            return null;
        }

        #endregion

        public async void GetLogsGame()
        {
            if (CheckInternetConnection("google.com") == false)
                return;
            if (UUID.Length != 0)
            {
                string URL = $"https://2025.nti-gamedev.ru/api/games/{UUID}/logs/\r\n";

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL);
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                Debug.Log(await response.Content.ReadAsStringAsync());
            }

        }

        public bool CheckInternetConnection(string nameOrAddress)
        {
            try
            {
                using (System.Net.NetworkInformation.Ping pinger = new System.Net.NetworkInformation.Ping())
                {
                    PingReply reply = pinger.Send(nameOrAddress);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                Debug.LogWarning("��� ����������� � ��������� !");
                OnNotInternet?.Invoke();
                return false;
            }
        }
    }
}
