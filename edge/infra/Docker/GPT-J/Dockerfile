FROM python:3.10

RUN pip install flask transformers torch

COPY . /app

WORKDIR /app

EXPOSE 5000

CMD ["python", "app.py"]