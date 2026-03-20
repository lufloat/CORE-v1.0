import random

def decidir(estado):
    comida = estado["comida"]
    moral = estado["moral"]
    tecnologia = estado["tecnologia"]

    if comida < 20:
        return "ProduzirComida"
    if moral < 30:
        return "MelhorarMoral"
    if tecnologia < 20:
        return "InvestirTecnologia"

    return random.choice([
        "TreinarExercito",
        "ProduzirComida",
        "InvestirTecnologia"
    ])