using System.IO.Compression;
using System.Text;
using NetMQ;
using NetMQ.Sockets;

class Subscriber
{
    //string serverUrl = "tcp://pubsub.besteffort.ndovloket.nl:7817";}
    string serverUrl = "tcp://localhost:8888";
    //string[] topics = ["/RIG/KV6posinfo", "/RIG/KV17cvlinfo"];
    string[] topics = ["/RIG/KV6posinfo"];

    public async Task Run()
    {
        // create a zeromq connection to the server
        using (var subscriber = new SubscriberSocket())
        {
            subscriber.Connect(serverUrl);

            // subscribe to the list of topics provided in a constant
            foreach (var t in topics)
            {
                subscriber.Subscribe(t);
            }

            while (true)
            {
                Console.WriteLine("Processing message");
                var msg = await subscriber.ReceiveMultipartMessageAsync();
                var topic = msg[0].ConvertToString();
                string decompressed = "";
                if (msg.FrameCount < 3)
                {
                    decompressed = GZipDecompress(msg.Last.Buffer);
                }
                else
                {
                    // combine multiple frames then decompress those
                }

                Console.WriteLine(decompressed);
                //TODO: Pass deserialized XML and topic on to next step
            }
        }
    }

    public static string GZipDecompress(byte[] data)
    {
        using (MemoryStream output = new MemoryStream())
        using (MemoryStream input = new MemoryStream(data))
        using (GZipStream gzip = new GZipStream(input, CompressionMode.Decompress))
        {
            gzip.CopyTo(output);
            return Encoding.Default.GetString(output.ToArray());
        }
    }
}