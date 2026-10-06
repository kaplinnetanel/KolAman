import logging
from elasticsearch import Elasticsearch


elastic = Elasticsearch("http://localhost:9200")


class ElasticsearchHandler(logging.Handler):

    def emit(self, record):
        log = {
            "level": record.levelname,
            "message": record.getMessage()
        }

        try:
            elastic.index(
                index="python-logs",
                document=log
            )
        except Exception:
            pass


def get_logger(name):

    logger = logging.getLogger(name)

    if not logger.handlers:
        logger.setLevel(logging.DEBUG)

        file_handler = logging.FileHandler(
            "app.log",
            encoding="utf-8"
        )

        console_handler = logging.StreamHandler()

        elastic_handler = ElasticsearchHandler()

        formatter = logging.Formatter(
            "%(asctime)s - [%(name)s] - %(levelname)s - %(message)s"
        )

        file_handler.setFormatter(formatter)
        console_handler.setFormatter(formatter)
        elastic_handler.setFormatter(formatter)

        logger.addHandler(file_handler)
        logger.addHandler(console_handler)
        logger.addHandler(elastic_handler)

    return logger