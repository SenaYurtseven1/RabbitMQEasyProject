using RabbitMQ.Client;
using System.Text;

//Create a connection to the RabbitMQ server
ConnectionFactory connectionFactory = new();
connectionFactory .Uri = new Uri("amqp://guest:guest@localhost:5672");

//Activate the connection and open a channel
using IConnection connection = await connectionFactory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

//Create a queue
await channel.QueueDeclareAsync(queue: "example-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

//Sending a message
//byte[] message = Encoding.UTF8.GetBytes("Hello");
//await channel.BasicPublishAsync(exchange:"",routingKey: "example-queue", body: message);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Hello " + i);
    await channel.BasicPublishAsync(exchange:"",routingKey: "example-queue", body: message);

}

Console.Read();