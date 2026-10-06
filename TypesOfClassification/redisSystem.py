import redis
import logger as lg

logging = lg.get_logger("redis")

import redis


redis_client = redis.Redis(
    host="localhost",
    port=6379,
    decode_responses=True
)


def check_alert(alter_id):

    alert_key = f"{alter_id}"

    if redis_client.exists(alert_key):
        return False

    redis_client.set(
        alert_key,
        "1",
        ex=300
    )
    return True

