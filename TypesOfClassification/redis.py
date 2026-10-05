import redis
import logger as lg



logging = lg.get_logger("redis")

redis_client = redis.Redis(
    host="localhost",
    port=6379,
    decode_responses=True)

def checker_and_send(alart_id):
    if redis_client.exists(alart_id):
        logging.info("Duplicate alert")
    else:
        redis_client.set(alart_id, "1", ex=300)
        logging.info("No duplicate was found, and it was saved to Redis.")

