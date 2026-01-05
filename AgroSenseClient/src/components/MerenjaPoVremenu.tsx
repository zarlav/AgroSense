import { useState } from "react";
import "./MerenjaPoVremenu.css";

export interface MerenjaDto {
  datum: string;
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

export default function MerenjaPoDanu() {
    const [idSenzora, setIdSenzora] = useState("");
    const [dan, setDan] = useState("");
    const [vremeOd, setVremeOd] = useState("");
    const [vremeDo, setVremeDo] = useState("");
    const [merenjaPoDanu, setMerenjaPoDanu] = useState<MerenjaDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError]  = useState<string | null>(null);

    const fetchMerenjaPoDanu = () => {
        if(!idSenzora) {
            setError("Unesi ID senzora!");
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

        fetch(`https://localhost:7025/api/Merenja/po_vremenu?senzorId=${idSenzora}&dan=${dan}&vremeOd=${vremeOd}&vremeDo=${vremeDo}`)
            .then( res => {
                if(!res.ok)
                    throw new Error("Greska pri ucitavanju merenja po vremenu!");
                    return res.json();
            })
            .then((data:MerenjaDto[]) => setMerenjaPoDanu(data))
            .catch(err => setError(err.message))
            .finally(() => setLoading(false));
    };

    return(
        <div className="merenjaPoDanu">
            <div className="merenjaPoDanu-Forma">
                <output></output>
                <input
                 type="text"
                 placeholder="Unesite ID senzora"
                 value={idSenzora}
                onChange={(e) =>setIdSenzora(e.target.value)}
                className="merenjePoDanuInput"
                ></input>
                <input
                type="text"
                 placeholder="Unesite dan"
                 value={dan}
                onChange={(e) =>setDan(e.target.value)}
                className="merenjePoDanuInput"
                ></input>
                <input
                type="text"
                 placeholder="Vreme od:"
                 value={vremeOd}
                onChange={(e) =>setVremeOd(e.target.value)}
                className="merenjePoDanuInput"
                ></input>
                <input
                type="text"
                 placeholder="Vreme do:"
                 value={vremeDo}
                onChange={(e) =>setVremeDo(e.target.value)}
                className="merenjePoDanuInput"
                ></input>
                <button onClick={fetchMerenjaPoDanu} className="merenjaPoDanu-btn">
                    Prikazi merenja
                </button>
            </div>
            {loading && <p className="merenja-loading">Ucitavanje...</p>}
            {error && <p className="merenja-error">{error}</p>}
  {merenjaPoDanu.length > 0 && (
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
              {merenjaPoDanu.map((s, index) => (
                <tr key={index}>
                  <td>{new Date(s.datum).toLocaleDateString()}</td>
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