using RabbitMQ.Client;
using System.Text;

//Create a connection to the RabbitMQ server
ConnectionFactory connectionFactory = new();
connectionFactory .Uri = new Uri("amqp://guest:guest@localhost:5672");

//Activate the connection and open a channel
using IConnection connection = await connectionFactory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

//Create a queue
//Durable: false means that the queue will not be durable and will not be saved to disk
//Durable: true means that the queue will be durable and will be saved to disk
//If durable parameter is true then you have to create properties and change the persistent parameter to true
//Also use in BasicPublish method
//await channel.QueueDeclareAsync(queue: "example-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

//Sending a message
//byte[] message = Encoding.UTF8.GetBytes("Hello");
//await channel.BasicPublishAsync(exchange:"",routingKey: "example-queue", body: message);

//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes("Hello " + i);
//    await channel.BasicPublishAsync(exchange:"",routingKey: "example-queue", body: message);

//}


//Direct exchange -- not use queue
//Create a exchange
await channel.ExchangeDeclareAsync(exchange: "direct-example-exchange", type: ExchangeType.Direct, durable: false, autoDelete: false, arguments: null);


    Console.Write("Message : ");
    string message = Console.ReadLine();
    byte[] body = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(exchange: "direct-example-exchange", routingKey: "direct-queue-example", body: body);


Console.Read();