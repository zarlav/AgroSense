import random
import time
from datetime import datetime

import requests


API_URL = "https://localhost:7025/api/Merenja"
SENZORI_URL = "https://localhost:7025/api/Senzor/svi_senzori_ids"


def generisi_temperaturu(last):
    if last == -100 or last < -30 or last > 42:
        return round(random.uniform(-30, 42), 2)
    return round(random.uniform(last - 0.3, last + 0.3), 2)

def generisi_vlaznost(last):
    if last == -100:
        return random.randint(0, 100)
    return max(0, min(100, random.randint(last - 2, last + 2)))

def generisi_co2(last):
    if last == -100:
        return random.randint(380, 460)
    return random.randint(last - 2, last + 2)

def generisi_uv(last):
    if last == -100:
        return round(random.uniform(0, 10), 2)
    return round(random.uniform(last - 0.1, last + 0.1), 2)

def generisi_jacinuVetra(last):
    if last == -100:
        return random.randint(0, 10)
    return round(random.uniform(max(0, last - 0.2), last + 0.2), 2)

def generisi_smerVetra():
    return random.choice(['S','N','E','W','SW','SE','NE','NW'])

def generisi_phZemljista(last):
    if last == -100:
        return round(random.uniform(6.1, 7.1), 2)
    return round(random.uniform(last - 0.05, last + 0.05), 2)

def generisi_pritisakVazduha(last):
    if last == -100:
        return random.randint(960, 1060)
    return random.randint(last - 1, last + 1)

def generisi_pritisakUCevima(last):
    if last == -100:
        return round(random.uniform(2.1, 4.9), 2)
    return round(random.uniform(last - 0.06, last + 0.06), 2)

def ucitaj_senzore():
    try:
        r = requests.get(SENZORI_URL, verify=False)
        r.raise_for_status()
        data = r.json()     
        if not data:         
            print("Nema senzora.")
            return None
        return data
    except requests.exceptions.RequestException as e:
        print(f"Greska pri ucitavanju senzora {e}")
        return None
    except ValueError:
        print("Odgovor nije validan JSON.")
        return None
senzori = ucitaj_senzore()
    
if senzori is None:
     print("Nema senzora !!!")
     stanja = {}
else:
    stanja = {
        s["senzorId"]: {
            "lokacijaId": s["lokacijaId"],
            "temp": -100,
            "vlaznost": -100,
            "co2": -100,
            "vetar": -100,
            "ph": -100,
            "uv": -100,
            "pritisak_v": -100,
            "pritisak_c": -100
        }
        for s in senzori
}

def posalji_merenje(senzor_id):
    print("Nema senzora, inicijalizacija stanja preskočena.")
    s = stanja[senzor_id]

    s["temp"] = generisi_temperaturu(s["temp"])
    s["vlaznost"] = generisi_vlaznost(s["vlaznost"])
    s["co2"] = generisi_co2(s["co2"])
    s["vetar"] = generisi_jacinuVetra(s["vetar"])
    s["ph"] = generisi_phZemljista(s["ph"])
    s["uv"] = generisi_uv(s["uv"])
    s["pritisak_v"] = generisi_pritisakVazduha(s["pritisak_v"])
    s["pritisak_c"] = generisi_pritisakUCevima(s["pritisak_c"])

    trenutniDatum = datetime.now()
    merenje = {
        "id_senzora": senzor_id,
    "id_lokacije": s["lokacijaId"],
        "datum": {
            "year": trenutniDatum.year,
            "month": trenutniDatum.month,
            "day": trenutniDatum.day
        },
        "ts": datetime.now().isoformat(),
        "temperatura": s["temp"],
        "vlaznost": s["vlaznost"],
        "co2": s["co2"],
        "jacina_vetra": s["vetar"],
        "smer_vetra": generisi_smerVetra(),
        "ph_zemljista": s["ph"],
        "uv_vrednost": s["uv"],
        "pritisak_vazduha": s["pritisak_v"],
        "pritisak_u_cevima": s["pritisak_c"]
    }

    response = requests.post(API_URL, json=merenje, verify=False)

    if response.ok:
        print(f"[{senzor_id[:8]}] ...  upisano merenje! ")
    else:
        print(f"[{senzor_id[:8]}] {response.status_code}")
        print(response.text)
    print(merenje)

while True:
    for senzor_id in stanja.keys():
        posalji_merenje(senzor_id)

    time.sleep(15)
