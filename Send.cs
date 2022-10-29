using System;
using RabbitMQ.Client;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

class Send
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var conn = factory.CreateConnection())
        {
            using(var channel = conn.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
                VideoCapture videoCapture = new VideoCapture();

                while(true)
                {
                Mat item = videoCapture.QueryFrame();
                var img = item.ToImage<Bgr, byte>().ToJpegData();
                var bytes = Convert.ToBase64String(img);

                var body = Encoding.UTF8.GetBytes(bytes);
                channel.BasicPublish(exchange: "",
                routingKey: "hello",
                basicProperties: null,
                body: body);
                }

                //Console.WriteLine("[x] sent{0} ", img.Size.ToString());
            }
        }

        Console.WriteLine(DateTime.Now.TimeOfDay);
        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();
    }
}