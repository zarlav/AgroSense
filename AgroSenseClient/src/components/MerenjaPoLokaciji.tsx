import { useEffect, useState } from "react";
import "./MerenjaPoVremenu.css";

export type LokacijeDto = string;
export interface MerenjaDto {
  datum: { day: number; month: number; year: number };
  ts: string;
  temperatura: number;
  vlaznost: number;
  co2: number;
  jacina_vetra: number;
  smer_vetra: string;
  ph_zemljista: number;
  uv_vrednost: number;
  vlaznost_lista: number;
  pritisak_vazduha: number;
  pritisak_u_cevima: number;
}

export default function MerenjaPoLokaciji() {
    const [lokacije, setLokacije] = useState<LokacijeDto[]>([]);
    const [lokacijaId, setIdLokacije] = useState("");
    const [dan, setDan] = useState("");
    const [vremeOd, setVremeOd] = useState("");
    const [vremeDo, setVremeDo] = useState("");
    const [merenjaPoLokaciji, setMerenjaPoLokaciji] = useState<MerenjaDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
useEffect(() => {
  fetch("https://localhost:7025/api/ProizvodneJedinice/ids")
    .then(res => {
      if(!res.ok) throw new Error("Greska pri ucitavanju id-eva jedinica!");
      return res.json();
    })
    .then((data: string[]) => setLokacije(data))
    .catch(err => setError(err.message));
}, []);

    const fetchMerenjaPoLokaciji = () => {
        if(!lokacijaId) {
            setError("Unesite ID lokacije");
            return;
        }
        else if(!dan) {
            setError("Unesite datum u formatu yyyy-mm-dd");
            return;
        }
           else if(!vremeOd) {
            setError("Unesite vreme u formatu hh:mm");
            return;
        }
        else if(!vremeDo) {
            setError("Unesite vreme u formatu hh:mm");
            return;
        }

        setLoading(true);
        setError(null);
    fetch(`https://localhost:7025/api/Merenja/po_lokaciji?lokacijaId=${lokacijaId}&dan=${dan}&vremeOd=${vremeOd}&vremeDo=${vremeDo}`)
            .then( res => {
                if(!res.ok)
                    throw new Error("Greska pri ucitavanju merenja po lokaciji!");
                    return res.json();
            })
            .then((data:MerenjaDto[]) => setMerenjaPoLokaciji(data))
            .catch(err => setError(err.message))
            .finally(() => setLoading(false));
    };
return(
        <div className="merenjaPoDanu">
            <div className="merenjaPoDanu-Forma">
                <output></output>
               <select
              value={lokacijaId}
              onChange={(e) => setIdLokacije(e.target.value)}
              className="merenjePoDanuInput">
              <option value="">Izaberite proizvodnu jedinicu</option>
                {lokacije.map((id) => (
                  <option key={id} value={id}>{id}</option>
                  ))}
              </select>
                <input
                type="date"
                 placeholder="Izaberite dan"
                 value={dan}
                onChange={(e) =>setDan(e.target.value)}
                className="merenjePoDanuInput"
                ></input>
                <input
                type="time"
                 placeholder="Vreme od:"
                 value={vremeOd}
                onChange={(e) =>setVremeOd(e.target.value)}
                className="merenjePoDanuInput"
                ></input>
                <input
                type="time"
                 placeholder="Vreme do:"
                 value={vremeDo}
                onChange={(e) =>setVremeDo(e.target.value)}
                className="merenjePoDanuInput"
                ></input>
                <button onClick={fetchMerenjaPoLokaciji} className="merenjaPoDanu-btn">
                    Prikazi merenja
                </button>
            </div>
            {loading && <p className="merenja-loading">Ucitavanje...</p>}
            {error && <p className="merenja-error">{error}</p>}
  {merenjaPoLokaciji.length > 0 && (
        <div className="merenja-table-wrapper">
          <table className="merenja-table">
            <thead>
              <tr>
                <th>Datum</th>
                <th>Vreme</th>
                <th>Temperatura</th>
                <th>Vlažnost</th>
                <th>CO₂</th>
                <th>Smer vetra</th>
                <th>Jačina vetra</th>
                <th>pH zemljišta</th>
                <th>UV vrednost</th>
                <th>Vlažnost lista</th>
                <th>Pritisak vazduha</th>
                <th>Pritisak u cevima</th>
              </tr>
            </thead>
            <tbody>
              {merenjaPoLokaciji.map((s, index) => (
                <tr key={index}>
                  <td>{new Date(s.datum.year, s.datum.month - 1, s.datum.day).toLocaleDateString()}</td>
                  <td>{new Date(s.ts).toLocaleTimeString()}</td>
                  <td>{s.temperatura}</td>
                  <td>{s.vlaznost}</td>
                  <td>{s.co2}</td>
                  <td>{s.smer_vetra}</td>
                  <td>{s.jacina_vetra}</td>
                  <td>{s.ph_zemljista}</td>
                  <td>{s.uv_vrednost}</td>
                  <td>{s.vlaznost_lista}</td>
                  <td>{s.pritisak_vazduha}</td>
                  <td>{s.pritisak_u_cevima}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}