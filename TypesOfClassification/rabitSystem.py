# import json 
# import pika


# # def send_to_rabit(message):
# #     connection = pika.BlockingConnection(
# #         pika.ConnectionParameters("localhost")
# #     )


# #     def send_By_rabit(message):
        
# #         channel = connection.channel()

# #         channel.queue_declare(queue="alerts")

# #         channel.basic_publish(
# #         exchange="",
# #         routing_key="alerts",
# #         body=json.dumps(message)
# #         )

# #         connection.close()