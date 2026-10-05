import os
import json
from confluent_kafka import Consumer
def main():
    consumer = Consumer({
        "bootstrap.servers": os.getenv(
            "KAFKA_BOOTSTRAP_SERVERS",
            "localhost:9092"
        ),
        "group.id": "consumer",
        "auto.offset.reset": "earliest"
    })

    consumer.subscribe(["Warning"])

    while True:
        message = consumer.poll(1.0)

        if message is None:
            continue

        if message.error():
            print(message.error())
            continue
        new = message

        print(new)
        try:
            data = json.loads(
                message.value().decode("utf-8")
            )

            print("Valid:", data)

        except (json.JSONDecodeError, ValueError) as e:
            print("Invalid data:", e)
            
if __name__ == "__main__":
       main()           

