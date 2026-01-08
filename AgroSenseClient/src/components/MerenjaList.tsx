import { useState, useEffect } from "react";
import "./MerenjaList.css";

export interface SenzorDto {
  senzorId: number;
  naziv: string;
}

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

export default function MerenjaList() {
  const [senzori, setSenzori] = useState<SenzorDto[]>([]);
  const [idSenzora, setIdSenzora] = useState("");
  const [merenja, setMerenja] = useState<MerenjaDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        fetch("https://localhost:7025/api/Senzor/svi_senzori")
          .then(res => {
              if(!res.ok) throw new Error("Greska pri ocitavanju id-jeva senzora!");
              return res.json();
          })
          .then((data: SenzorDto[]) => setSenzori(data))
          .catch(err => setError(err.message));
    }, []);

  const fetchMerenja = () => {
    if (!idSenzora) {
      setError("Unesite ID senzora!");
      return;
    }

    setLoading(true);
    setError(null);

    fetch(`https://localhost:7025/api/Merenja/poslednje_merenje?senzorId=${idSenzora}`)
      .then(res => {
        if (!res.ok) throw new Error("Greska pri ocitavanju posglednjeg merenja!");
        return res.json();
      })
      .then((data: MerenjaDto[]) => setMerenja(data))
      .catch(err => setError(err.message))
      .finally(() => setLoading(false));
  };

  return (
    <div className="merenja">
      <div className="merenja-form">
        <output></output>
         <select
            value={idSenzora}
              onChange={(e) => setIdSenzora(e.target.value)}
              className="merenjePoDanuInput">
                  <option value="">Izaberite senzor</option>
                    {senzori.map((s) => (
                        <option key={s.senzorId} value={s.senzorId}>
                            {s.naziv} (ID: {s.senzorId})
                        </option>
                    ))}
            </select>
        <button onClick={fetchMerenja} className="merenja-btn">
          Prikazi merenje
        </button>
      </div>

      {loading && <p className="merenja-loading">Ucitavanje...</p>}
      {error && <p className="merenja-error">{error}</p>}

      {merenja.length > 0 && (
        <div className="merenja-table-wrapper">
          <table className="merenja-table">
            <thead>
              <tr>
                <th>Datum</th>
                <th>Vreme</th>
                <th>Temperatura</th>
                <th>Vlaznost</th>
                <th>CO2</th>
                <th>Smer vetra</th>
                <th>Jacina vetra</th>
                <th>pH zemljista</th>
                <th>UV vrednost</th>
                <th>Vlaznost lista</th>
                <th>Pritisak vazduha</th>
                <th>Pritisak u cevima</th>
              </tr>
            </thead>
            <tbody>
              {merenja.map((s, index) => (
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
