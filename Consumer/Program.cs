﻿//Create a connection to the RabbitMQ server
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory connectionFactory = new();
connectionFactory.Uri = new Uri("amqp://guest:guest@localhost:5672");

//Activate the connection and open a channel
using IConnection connection = await connectionFactory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

//Create a queue
//await channel.QueueDeclareAsync(queue: "example-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

//Read a message
//AutoAck: true means that the message will be automatically acknowledged after being received and deleted
//AutoAck: false means that the message will not be automatically acknowledged after being received and deleted
//AsyncEventingBasicConsumer consumer = new(channel);
//await channel.BasicConsumeAsync(queue: "example-queue", autoAck: true, consumer: consumer);
//Set the prefetch count to 1, which means that only one message will be sent to the consumer at a time
//prefetchSize: 0 means that the prefetch size is not limited
//await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, false); 
//consumer.ReceivedAsync += async (sender, ea) =>
//{
//    //The place that the message is received and processed
//    //e.Body: the all arguments in message that is being processed
//    //e.Body.Span or e.Body.ToArray(): the byte message that is being processed in
//    byte[] body = ea.Body.ToArray();
//    string message = System.Text.Encoding.UTF8.GetString(body);
//    Console.WriteLine($"Received message: {message}");
//    //If AutoAck is false, the message will not be automatically acknowledged after being received and deleted
//    //DeliveryTag: the unique identifier of the message that is being processed
//    //multiple: true means that all messages that are being processed will be acknowledged
//    //await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
//};

#region Direct Exchange
////Direct exchange 
////Same name with publisher exchange
//await channel.ExchangeDeclareAsync(exchange: "direct-example-exchange", type: ExchangeType.Direct, durable: false, autoDelete: false, arguments: null);

////Create a queue which is same name with publisher queue
//string queueName = (await channel.QueueDeclareAsync()).QueueName;

////Bind the queue to the exchange
//await channel.QueueBindAsync(queue: queueName, exchange: "direct-example-exchange", routingKey: "direct-queue-example");

////Recieve a message
//AsyncEventingBasicConsumer consumer = new(channel); 
//await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
//consumer.ReceivedAsync += async (sender, ea) =>
//{
//    //The place that the message is received and processed
//    //e.Body: the all arguments in message that is being processed
//    //e.Body.Span or e.Body.ToArray(): the byte message that is being processed in
//    byte[] body = ea.Body.ToArray();
//    string message = System.Text.Encoding.UTF8.GetString(body);
//    Console.WriteLine($"Received message: {message}");
//};  

//Console.Read();
#endregion

#region Fanout Exchange
////Fanout exchange
//await channel.ExchangeDeclareAsync(exchange: "fanout-example-exchange", type: ExchangeType.Fanout, durable: false, autoDelete: false, arguments: null);

//Console.WriteLine("Enter the queue name: ");
//string queueName = Console.ReadLine();
//await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
//await channel.QueueBindAsync(queue: queueName, exchange: "fanout-example-exchange", routingKey: "");

//AsyncEventingBasicConsumer consumer = new(channel); 
//await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
//consumer.ReceivedAsync += async (sender, ea) =>
//{
//    //The place that the message is received and processed
//    //e.Body: the all arguments in message that is being processed
//    //e.Body.Span or e.Body.ToArray(): the byte message that is being processed in
//    byte[] body = ea.Body.ToArray();
//    string message = System.Text.Encoding.UTF8.GetString(body);
//    Console.WriteLine($"Received message: {message}");
//};
#endregion

#region TopicExchange

await channel.ExchangeDeclareAsync(exchange: "topic-example-exchange", type: ExchangeType.Topic, durable: false, autoDelete: false, arguments: null);
Console.Write("Please give topic format: ");
string topic = Console.ReadLine();
string queueName = (await channel.QueueDeclareAsync()).QueueName;
await channel.QueueBindAsync(queue: queueName, exchange: "topic-example-exchange", routingKey: topic);

AsyncEventingBasicConsumer consumer = new(channel);
await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
consumer.ReceivedAsync += async (sender, ea) =>
{
    //The place that the message is received and processed
    //e.Body: the all arguments in message that is being processed
    //e.Body.Span or e.Body.ToArray(): the byte message that is being processed in
    byte[] body = ea.Body.ToArray();
    string message = System.Text.Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received message: {message}");
};
#endregion
Console.Read();