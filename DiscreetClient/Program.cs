using System;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiscreetClient
{
    public static class IEnumerable
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }

    class Program
    {

        static async Task DoWork()
        {
            int i = 0;
            await Task.Run(async () =>
            {
                using (var webClient = new WebClient())
                {
                    while (true)
                    {
                        Random r = new Random();
                        int rv = r.Next(0, 1000);
                        string guids = Guid.NewGuid().ToString();
                        Console.WriteLine("--> {\"jsonrpc\": \"2.0\", \"method\": \"test_getStealthAddress\", \"id\": " + rv + "}");
                        var result = webClient.UploadString($"http://localhost:8350/", "POST", "{\"jsonrpc\": \"2.0\", \"method\": \"getStealthAddressTest\", \"id\": \"" + rv + "\"}");
                        Console.WriteLine("<-- " + result);
                        Console.Title = i.ToString();
                        i++;
                    }
                }
            });
        }


        public static uint NUMSPACE = 4;
        private static readonly Regex sWhitespace = new Regex(@"\s+");

        

        public static string Prettify(string s)
        {
            s = sWhitespace.Replace(s, "");

            int nBrace = 0;
            StringBuilder rv = new();

            string spaces = "";

            bool quoted = false;

            for (int i = 0; i < NUMSPACE; i++)
            {
                spaces += " ";
            }

            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        rv.Append(ch);
                        if (!quoted)
                        {
                            rv.AppendLine();
                            Enumerable.Range(0, ++nBrace).ForEach(item => rv.Append(spaces));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            rv.AppendLine();
                            Enumerable.Range(0, --nBrace).ForEach(item => rv.Append(spaces));
                        }
                        rv.Append(ch);
                        break;
                    case '"':
                        rv.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && s[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        rv.Append(ch);
                        if (!quoted)
                        {
                            rv.AppendLine();
                            Enumerable.Range(0, nBrace).ForEach(item => rv.Append(spaces));
                        }
                        break;
                    case ':':
                        rv.Append(ch);
                        if (!quoted)
                        {
                            if (i < s.Length - 1 && (s[i + 1] == '[' || s[i + 1] == '{'))
                            {
                                rv.AppendLine();
                                Enumerable.Range(0, nBrace).ForEach(item => rv.Append(spaces));
                            }
                            else
                            {
                                rv.Append(' ');
                            }
                        }
                        break;
                    default:
                        rv.Append(ch);
                        break;
                }

            }

            return rv.ToString();
        }

        static async Task Main(string[] args)
        {
            // Benchmarking code
            //int i = 0;
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            //List<Task> tasks = new List<Task>() { DoWork(), DoWork() };
            //foreach (Task t in tasks)
            //{
            //    await t;
            //}

            //var data = "{\"Version\":1,\"Timestamp\":637800362343106364,\"Height\":1,\"Fee\":0,\"PreviousBlock\":\"ded13a3a87380d72eec51a53956af44090fd0087423517b6c04290afce0f3fb5\",\"BlockHash\":\"b80b316f1d06a079fe9ef1e8ba27f107e271959b6baf3e934dc486db0c311094\",\"MerkleRoot\":\"6cefb0a2828690665003b6fbbb1a770acebe2a99c4deed35d6dbb73627cf2811\",\"NumTXs\":1,\"BlockSize\":2077,\"NumOutputs\":1,\"Signature\":\"2c80fb6928db0ce31613bdaa6e3426f28599831976bb76a9f192490a7034db01b2b5cffab170652090315e3b8f562e3005b3f8f94c5470c420c05d59bfb2490f74df105d0d37ef0c31ef2656297e514c52ec49ce387b587f97a13e2c3a57065e\",\"transactions\":[{\"Version\":2,\"NumInputs\":1,\"NumOutputs\":1,\"NumSigs\":1,\"NumTInputs\":0,\"NumPInputs\":1,\"NumTOuputs\":0,\"NumPOutputs\":1,\"Fee\":0,\"TransactionKey\":\"6592892e19f114911a6036d1e998e7c93107c5eee66d6855cac6241dc27824fc\",\"PInputs\":[{\"Offsets\":[497,923,2543,432,3240,2213,359,381,537,3000,1261,627,653,2449,114,346,1141,26,1864,392,2938,1946,3082,100,2047,2675,384,713,237,3851,3267,716,404,2424,3864,531,434,558,2823,570,543,3526,475,1514,612,1721,742,292,686,681,37,1479,283,606,1209,1941,536,2525,677,534,689,556,207,4097],\"KeyImage\":\"b806e155be98dab121c92f122c8bb38347efa98bf62d8351165f7f2bfe4c8429\"}],\"POutputs\":[{\"UXKey\":\"63f92d82803a9b6f628dd206d32ba855575c7ec66a4381142beb93a44763b72f\",\"Commitment\":\"268f7ab6ea048918dfe3c22bc7edd27a58e920eed2c167d69f06da60eb01c87f\",\"Amount\":13771024707676076138}],\"RangeProofPlus\":{\"size\":1,\"A\":\"f7fa562bad800f88a8207538d8ec10aa2f1c3586c5c48fdf278829aed4e84232\",\"A1\":\"99b6bd46ffec6b3811685f6e4122963a00cad0642acd51c41e4ec4c1413387b5\",\"B\":\"de754859ed888ad955db403985349281cf2eae79cdbf6c21fa760c9b4d24ddff\",\"r1\":\"667c0e11315500816ef4bba7d2429c548f71dee41025fca48348eb8445138d04\",\"s1\":\"d2e99729d3f68ffd06eb3d34d850113675481efd65a0ab03b5df53b3b1cc3409\",\"d1\":\"e5b5a2beb43387387cb86bd214850c6fea2d94bd226809f156629214ae09c500\",\"L\":[\"87278f605da3594360c536a57f897768a9dea10600f454b8f3b3320050f0140a\",\"9cb62db08684a903b14d1b4ae4030d94b609880c273b3aa672e6c0092cd76f86\",\"e4bd1e36bb9e4baa34ffa653ffbe8dbe0787d86bc229e6add5e67bb24f0158cf\",\"91c1916c647a24db06fec0d77b04c222825ef92f5e18d1fb32a7ac1b1eb8bcda\",\"bb2a0b4af0d4dd1d818dfb630d7c779af45c6645e04e92657e83d827639c5550\",\"fb1679417a9866ca53d1ca7ae70c7043b877fa9a6d1eec9c518d19ef8d15d703\"],\"R\":[\"e54515fd5e91a4b320beed3ecc24ca33b2e6467e53e48a7b20596817a88e330a\",\"10fdbbb34feddcd0278eb6e9a3431fbd480216be5f197bbec9805fba125aa604\",\"bd4b9b50f562d70f18036ce330316850eb53fbfed4b1826bdce5f930b6fa0e7f\",\"1d5963408c1dcf56e6eeabf2d6221849d1c0e644003bc8b149866748238aff8f\",\"86f0c45ec9130b4e8c70f0d7efbb90205b806871251e218e4c9a6a58c70af9be\",\"e91c049ecf678586d9a058972056b59e2886ef5b107376b029c1dfedc370e46e\"]},\"PSignatures\":[{\"K\":\"01d8505f9c35f9bc2ee8127196a950a16da06b0b58e3d9419b38af9eef20c9da\",\"A\":\"f5be4dbf59ae8eeb5fbc91063ecfcd057cd529a39a36646d815f709bab9c9c3b\",\"B\":\"2f9b72e196ab1771daa0d21f884f0856fafa1c89a3d01e2362055e3f22fd39c2\",\"C\":\"84acf71fdd0da666acc48fe08bb2415555607267e9f0b8082f2f1883ba3e519f\",\"D\":\"236b6abcdcc161c819bc36ea076b27dc11ee1f31a011b3985849312774318c54\",\"X\":[\"7a1e0e11efba8f03a61bbe559850136fb236a819f43e6deb39db0cced575cba6\",\"c99a9587ca3a9acd994dffcc2a64242aa79ba1001a486d7afc844806f4832611\",\"f1192a1df38a7e2df7f343cacb659ec931f5ae0550f533a907efd830233cebaf\",\"0d5c4a68937ab01ae06d4e2b39795decb211eae977b3e57a545bf4f9d9d0541e\",\"1f0e46a2582ca803e336ea77b4a5f51a6e1b6978959bed123b30692f17abe0da\",\"4b1cec92986d9d64d2779b4d83b71c9a5956fc63fb15826da432555cadc42824\"],\"Y\":[\"f13bcc2c45086f0baa784300ee1af3ba34f8a561befe1b94e1f56b9b1b6d8a6b\",\"ee1a915aae7e7d3088387c8e38045f1bf265dd1825e039624db1b0874d02efbd\",\"e1c9b60a8ff7ac8624525b7fcf0632ecc672599bfda80cc4ccee6ca11e7531aa\",\"264b1bb5b98ce90226659ce1aa48a0549a618eb16f2953af5bc4416184f7a39e\",\"1516557cb04802745fc6027ab08fd694041055a8405f38fd458b52aa48eea764\",\"2da063e808f607ebf74d545a684b22713b1478cd8bd2cd76865293a124dee09a\"],\"f\":[\"113528b82d8ae9652dbafc16d7164ad8aa71b698bc6b3b60bb5ce57f40adc703\",\"2b3300c64c26082fdda7883244b723d77418f3059f879d9601ac6006b77c1e05\",\"37f238bd6b91bba6c550370438bbbe4775b365841eaf6958577444838cdb8f07\",\"040aa92d8b3941965bcf9bfedf5262f2df8b4de7cd3388ffec99130b99ff7604\",\"7bd8e0322d55b6b1a606ccddac0994e94e6d1e9856bebfdbd32dbbb07c519f0d\",\"b35476480cdb3cef2c9835dda8527902c827fd3f80fd0d91aed0d808074cfe06\"],\"zA\":\"2ec822ae1a88bfd1e0ae34b3936ddaaf9af9760b72135db36af276a8db6feb0e\",\"zC\":\"37d7a578b01d4b76b37955120fb1911c64411bd025d05477ddf63d466a7e7e02\",\"z\":\"116da0f7201e148086df936274a0011086e75283e6c25ccc84171df6ccbb2e09\"}],\"PseudoOutputs\":[\"268f7ab6ea048918dfe3c22bc7edd27a58e920eed2c167d69f06da60eb01c87f\"]}]}";

            //Console.WriteLine(Prettify(data));

            //return;

            //stopwatch.Stop();

            int i = 0;
            using (var webClient = new WebClient())
            {
                while (true)
                {
                    Console.Write("command --> ");

                    string _command = Console.ReadLine();

                    string _destination = "localhost";

                    //if (_command.StartsWith("dbg"))
                    //{
                        Console.Write("destination --> ");

                        _destination = Console.ReadLine();
                    //}

                    Console.Write("\nparams --> ");

                    string _params = $"[{Console.ReadLine()}]";

                    Random r = new Random();
                    int rv = r.Next(0, 1000);

                    string _call = $"{{\"jsonrpc\": \"2.0\", \"method\": \"{_command}\", \"params\": {_params}, \"id\": {rv} }}";

                    Console.WriteLine($"sending call: \n{_call}\n\n\n");

                    var result = webClient.UploadString($"http://{_destination}:8350", "POST", _call);
                    Console.WriteLine("Received data: " + result + "\n\n");
                }
            }

            //  Console.WriteLine($"{tasks.Count * 1000} calls done in {stopwatch.ElapsedMilliseconds}.");
        }
    }
}
