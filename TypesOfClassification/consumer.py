import os
import json
from confluent_kafka import Consumer
from redisSystem import checker_and_send
import logger as lg
from polygon import get_region_with_geopandas


def main():
    CLASSIFICATION = ["UNCLASSIFIED","RESTRICTED","SECRET","TOP_SECRET"]
    PRIORTY = ["Low","Medium","High","Critical"]
    consumer = Consumer({
        "bootstrap.servers": os.getenv(
            "KAFKA_BOOTSTRAP_SERVERS",
            "localhost:9092"
        ),
        "group.id": "consumer",
        "auto.offset.reset": "earliest"
    })

    logging = lg.get_logger("consumer")

    consumer.subscribe(["Warning"])

    while True:
        message = consumer.poll(1.0)

        if message is None:
            continue

        if message.error():
            print(message.error())
            continue

        try:

            data = json.loads(
                message.value().decode("utf-8")
            )
            
            print("Valid:", data)
            alert_id = data.get("alert_id")
            source = data.get("source")
            title = data.get("title")
            content = data.get("content")
            priority = data.get("priority")
            classification = data.get("classification")
            lat = data.get("lat")
            lon = data.get("lon")
            timestamp  = data.get("timestamp")
            status = data.get("status") 

            if status is None or timestamp is None or lon is None or lat is None or classification is None or priority is None or content is None or title is None or source is None or  alert_id is None:
                logging.error(alert_id ,"One of the fields is missing.")
                continue
            # if classification not in CLASSIFICATION:
            #     logging.error(alert_id,"classification not UNCLASSIFIED or RESTRICTED or SECRET or TOP_SECRET")
            #     continue

            # if priority not in PRIORTY:
            #     logging.error(alert_id,"PRIORTY not Low or Medium or High or Critical")
            #     continue

            # if (-180 <= lon <= 180 and  -90 <= lat <= 90):
            #     logging.error(alert_id, "-180 <= lon <= 180 and  -90 <= lat <= 90")
            #     continue
            checker_and_send(lat)
            region = get_region_with_geopandas("regions.geojson", lon,lat)
            print(region,"!!!!!!!!!")
        except (json.JSONDecodeError, ValueError) as e:

            print("Invalid data:", e)

if __name__ == "__main__":
       main()           

