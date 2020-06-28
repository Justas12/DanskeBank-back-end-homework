using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace homework
{
    class solver
    {
        private List<int[]> data;
        private List<Response> output;
        private List<Response> storage;
        private string path;
        public solver(string path)
        {
            this.path = path;
            this.output = new List<Response>();
            this.storage = LoadJSON();
        }
        public void populate(List<int[]> data)
        {
            this.data = data == null ? new List<int[]>() : data;
        }
        public void Solve()
        {
            foreach(int[] arr in data)
            {
                if (arr != null && arr.Length != 0)
                {
                    Response response;
                    if ((response = GetResponseFromStorage(arr)) == null)
                    {
                        response = new Response();
                        response.Arr = arr;
                        response.Path = new int[arr.Length];
                        response.Winnable = false;
                        List<int> tmp = new List<int>();
                        FindSolution(0, 0, arr.Length - 1, arr, response, tmp);
                        if (GetResponseFromStorage(arr) == null)
                        {
                            storage.Add(response);
                        }
                        response.Path = tmp.ToArray();
                    }
                    output.Add(response);
                }
            }
            SaveJSON();
        }

        private Response GetResponseFromStorage(int[] arr) 
        {
            if (storage != null)
            {
                foreach (Response res in storage)
                {
                    if (res.Arr.SequenceEqual(arr))
                    {
                        return res;
                    }
                }
            }
            return null;
        } 

        private void FindSolution(int pos, int index, int target, int[] arr, Response res, List<int> path)
        {
            for (int i = arr[index]; i >= 0; i--)
            {
                res.Path[pos] = arr[index];
                if (i + index == target)
                {
                    if (!res.Winnable)
                    {
                        res.Winnable = true;
                        storage.Add(res);
                        if (target > 0)
                        {
                            res.Path[++pos] = arr[target];
                        }
                        for (int j = 0; j <= pos; j++)
                        {
                            path.Add(res.Path[j]);
                        }
                    }
                }
                else if (i + index < target && i != 0)
                {
                    FindSolution(pos + 1, i + index, target, arr, res, path);
                }
            }
        }
        public List<Response> LoadJSON()
        {
            File.Open(path, FileMode.Append).Close();
            List<Response> items = new List<Response>();
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Response>>(json);
            }
            return items == null ? new List<Response>() : items;
        }
        private void SaveJSON()
        {
            using (StreamWriter w = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(w, storage);
            }
        }
        public List<Response> Output
        {
            get { return output; }
        }
        public List<Response> Storage
        {
            get { return storage; }
        }
    }
}
