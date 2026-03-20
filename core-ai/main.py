from fastapi import FastAPI
from ai import decidir

app = FastAPI()

@app.post("/decidir")
def decidir_rota(estado: dict):
    decisao = decidir(estado)
    return { "decisao": decisao }