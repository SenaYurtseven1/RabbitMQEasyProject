//Create a connection to the RabbitMQ server
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory connectionFactory = new();
connectionFactory.Uri = new Uri("amqp://guest:guest@localhost:5672");

//Activate the connection and open a channel
using IConnection connection = await connectionFactory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

//Create a queue
await channel.QueueDeclareAsync(queue: "example-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

//Read a message
AsyncEventingBasicConsumer consumer = new(channel);
await channel.BasicConsumeAsync(queue: "example-queue", autoAck: true, consumer: consumer);
consumer.ReceivedAsync += async (sender, ea) =>
{
    //The place that the message is received and processed
    //e.Body: the all arguments in message that is being processed
    //e.Body.Span or e.Body.ToArray(): the byte message that is being processed in
    byte[] body = ea.Body.ToArray();
    string message = System.Text.Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received message: {message}");
};


Console.Read();