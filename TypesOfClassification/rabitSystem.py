import pika
import json


connection = pika.BlockingConnection(
    pika.ConnectionParameters("localhost")
)

channel = connection.channel()


def send_to_rabbit(message, region):

    channel.queue_declare(
        queue=region
    )

    channel.basic_publish(
        exchange="",
        routing_key=region,
        body=json.dumps(message)
    )
