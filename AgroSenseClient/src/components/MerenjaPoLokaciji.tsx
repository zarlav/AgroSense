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

export default function MerenjaPoLokaciji() {
    const [lokacijaId, setIdSenzora] = useState("");
    const [dan, setDan] = useState("");
    const [merenjaPoLokaciji, setMerenjaPoLokaciji] = useState<MerenjaDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const fetchMerenjaPoLokaciji = () => {
        if(!lokacijaId) {
            setError("Unesite ID lokacije");
            return;
        }
        else if(!dan) {
            setError("Unesite datum u formatu yyyy-mm-dd");
            return;
        }

        setLoading(true);
        setError(null);
    fetch(`https://localhost:7025/api/Merenja/po_lokaciji?lokacijaId=${lokacijaId}&dan=${dan}`)
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
                <input
                 type="text"
                 placeholder="Unesite ID lokacije"
                 value={lokacijaId}
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