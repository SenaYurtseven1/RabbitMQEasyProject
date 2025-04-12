🧠 What is RabbitMQ?

What is a Message Queue?

A Message Queue is a design pattern used to enable communication between two independent systems. It provides a way to exchange data asynchronously.

🔁 This allows decoupling of producers and consumers, making the system more resilient and scalable.

RabbitMQ Overview

RabbitMQ is an open-source message broker that implements the AMQP (Advanced Message Queuing Protocol). It facilitates communication between applications by managing queues and routing messages appropriately.

🧩  Producer → Exchange → Queue → Consumer

📤 What is a Publisher / Producer?

A Publisher (or Producer) is the application that sends messages to an exchange in RabbitMQ.

📥 What is a Consumer?

A Consumer is the application that receives messages from a queue. It subscribes to the queue and processes incoming messages.

⚠️ Key Notes

The two systems exchanging messages can be written in entirely different programming languages or technologies.

A Message Broker is the broader structure that contains one or more message queues and manages communication.

A single message broker can contain multiple queues.

💬 What is AMQP?

AMQP (Advanced Message Queuing Protocol) is the messaging protocol RabbitMQ is based on. It defines how messages are formatted, transferred, and acknowledged between producers, brokers, and consumers.

🧱 What is an Exchange?

An Exchange receives messages from producers and routes them to queues based on rules called bindings and routing keys.

📍 What is Routing?

Routing is the process of delivering a message from an exchange to the appropriate queue(s) using a routing key.

🔗 What is a Binding?

A Binding connects an exchange to a queue and defines how messages should be routed.

📦 Exchange Types 

Direct Exchange: Routes messages with a specific routing key to the matching queue.

Fanout Exchange: Broadcasts messages to all queues bound to it, regardless of the routing key.

Topic Exchange: Routes messages to queues based on pattern matching with the routing key.

Headers Exchange: Uses message headers for routing instead of routing keys.

🛠️ Advanced Queue Architecture Notes

🔁 Round Robin Delivery

Messages are distributed evenly across multiple consumers (load balancing).

✅ Message Acknowledgment

Consumers must acknowledge that they’ve received and processed a message. If they don’t, the message can be re-queued or retried.

💾 Message Durability

Messages and queues can be configured to survive broker restarts (explained in detail in the next section).

📌 Note: Message Persistence in RabbitMQ

When working with RabbitMQ, making sure that messages are not lost after a broker restart requires more than just setting the queue to durable. True persistence involves specific configuration for both the queue and the messages.
## 📌 Note: Message Persistence in RabbitMQ

When working with RabbitMQ, making sure that messages are **not lost after a broker restart** requires more than just setting the queue to `durable`. True persistence involves specific configuration for both the **queue** and the **messages**.

---

### ✅ Two Required Steps for Message Persistence

#### 1. Declare the Queue as Durable

```csharp
channel.QueueDeclare(queue: "example-queue",
                     durable: true,        // Queue is not deleted after restart
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
```

#### 2. Mark the Message as Persistent

```csharp
var properties = channel.CreateBasicProperties();
properties.Persistent = true;  // Instruct RabbitMQ to write the message to disk

channel.BasicPublish(exchange: "",
                     routingKey: "example-queue",
                     basicProperties: properties,
                     body: body);
```

---

### ⚠️ Important Considerations

- `durable: true` only protects the **queue**, not the messages.
- `Persistent = true` asks RabbitMQ to write the message to disk, but doesn't **guarantee** it was written immediately.
- For stronger delivery guarantees, consider using **publisher confirms** or **transactions**.

---

## 🔄 Sync vs Async API Differences

### 🔹 Synchronous API (`IModel`)

- Supports all features like `CreateBasicProperties()`.
- Easier to configure for persistence.
- Recommended for most use cases where persistence is important.

```csharp
using IModel channel = connection.CreateModel();
var properties = channel.CreateBasicProperties();
properties.Persistent = true;

channel.BasicPublish(exchange: "", routingKey: "example-queue", basicProperties: properties, body: body);
```

### 🔹 Asynchronous API (`IChannel`)

- Does **not** support `CreateBasicProperties()`.
- Cannot directly mark messages as persistent.
- Designed for high-throughput scenarios.
- Requires more complex setup for persistence.

Example (no persistence):

```csharp
using IChannel channel = await connection.CreateChannelAsync();
await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: messageBytes);
```

---

> 🔀 **Recommendation**: If you require message persistence and your system does not have extremely high traffic, prefer using the synchronous API (`IModel`). The async API is better suited for high-performance scenarios but introduces complexity when handling message durability.
> 
⚖️ Fair Dispatch

Fair dispatch ensures that no single consumer is overloaded while others remain idle.
This is achieved by setting the prefetch count to 1 using the channel’s QoS (Quality of Service) settings. It tells RabbitMQ not to give more than one message to a worker at a time until it has acknowledged the previous one.

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

This makes sure that long-running tasks do not prevent other consumers from getting work and helps balance the load more fairly.
