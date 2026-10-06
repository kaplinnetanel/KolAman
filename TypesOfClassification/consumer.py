import os
import json
from confluent_kafka import Consumer
from redisSystem import check_alert
import logger as lg
from polygon import get_region_with_geopandas
from rabitSystem import send_to_rabbit


def main():
    CLASSIFICATION = ["UNCLASSIFIED","RESTRICTED","SECRET","TOP_SECRET"]
    PRIORTY = ["LOW","MEDIUM","HIGH","CRITICAL"]
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
            if ( status is None or timestamp is None or lon is None or lat is None or classification is None or priority is None or content is None or title is None or source is None or alert_id is None ): 
                logging.error( f"Alert {alert_id}: missing field" ) 
                continue 

            if classification not in CLASSIFICATION:
                 logging.error( f"Alert {alert_id}: invalid classification" )
                 continue
             
            if priority not in PRIORTY: 
                logging.error( f"Alert {alert_id}: invalid priority" )
                continue

            if type(lat) not in (int,float):
                logging.error( f"Alert {lat}: not int or float" )
                continue

            if type(lon) not in (int,float):
                logging.error( f"Alert {lon}: not int or float" )
                continue
        
            if not (-180 <= lon <= 180 and -90 <= lat <= 90):
                 logging.error( f"Alert {alert_id}: invalid coordinates" )
                 continue 
            
            if not check_alert(alert_id): 
                logging.info( f"Duplicate alert at {alert_id}" ) 
                continue 

            region = get_region_with_geopandas( "regions.geojson", lon, lat )

            logging.info( f"Alert {alert_id} classified to {region}" )
           
            send_to_rabbit(data, region) 
            logging.info( f"Alert {alert_id} sent to {region}" )
       
        except (json.JSONDecodeError, ValueError) as e: 
            logging.error( f"Invalid alert: {e}" )

if __name__ == "__main__":
    main()
