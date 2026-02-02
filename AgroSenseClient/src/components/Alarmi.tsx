import { useState } from "react";
import "./Alarmi.css";

interface AlarmDto {
  id_jedinice: string;
  id_senzora: string;
  dan: {
    year: number;
    month: number;
    day: number;
  };
  vreme_dogadjaja: string;
  parametar: string;
  trenutna_vrednost: number;
  granicna_vrednostMin: number;
  granicna_vrednostMax: number;
  komentar: string;
}

export default function Alarmi() {
  const [idJedinice, setIdJedinice] = useState("");
  const [dan, setDan] = useState("");
  const [vremeOd, setVremeOd] = useState("");
  const [vremeDo, setVremeDo] = useState("");
  const [alarmi, setAlarmi] = useState<AlarmDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchAlarmi = () => {
    if (!idJedinice || !dan || !vremeOd || !vremeDo) {
      setError("Popuni sva polja");
      return;
    }

    setLoading(true);
    setError(null);

    fetch(
      `http://localhost:5161/api/Alarm/po_jedinici` +
        `?idJedinice=${idJedinice}` +
        `&dan=${dan}` +
        `&vremeOd=${vremeOd}:00` +
        `&vremeDo=${vremeDo}:00`
    )
      .then((res) => {
        if (!res.ok) throw new Error("Nema alarma za dati period");
        return res.json();
      })
      .then((data: AlarmDto[]) => setAlarmi(data))
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  };

  return (
    <div className="alarmi">
      <h2>Alarmi po proizvodnoj jedinici</h2>

      <div className="alarmi-Forma">
        <div className="alarmi-Polje">
          <label>ID proizvodne jedinice</label>
          <input
            className="alarmiInput"
            value={idJedinice}
            onChange={(e) => setIdJedinice(e.target.value)}
            placeholder="GUID jedinice"
          />
        </div>

        <div className="alarmi-Polje">
          <label>Datum</label>
          <input
            className="alarmiInput"
            type="date"
            value={dan}
            onChange={(e) => setDan(e.target.value)}
          />
        </div>

        <div className="alarmi-Polje">
          <label>Vreme od</label>
          <input
            className="alarmiInput"
            type="time"
            value={vremeOd}
            onChange={(e) => setVremeOd(e.target.value)}
          />
        </div>

        <div className="alarmi-Polje">
          <label>Vreme do</label>
          <input
            className="alarmiInput"
            type="time"
            value={vremeDo}
            onChange={(e) => setVremeDo(e.target.value)}
          />
        </div>

        <button className="alarmi-btn" onClick={fetchAlarmi}>
          Prikaži alarme
        </button>
      </div>

      {loading && <p className="alarmi-loading">Učitavanje...</p>}
      {error && <p className="alarmi-error">{error}</p>}

      {alarmi.length > 0 && (
        <div className="alarmi-table-wrapper">
          <table className="alarmi-table">
            <thead>
              <tr>
                <th>Parametar</th>
                <th>Trenutna</th>
                <th>Min</th>
                <th>Max</th>
                <th>Vreme</th>
                <th>Komentar</th>
              </tr>
            </thead>
            <tbody>
              {alarmi.map((a, i) => (
                <tr key={i}>
                  <td>{a.parametar}</td>
                  <td>{a.trenutna_vrednost}</td>
                  <td>{a.granicna_vrednostMin}</td>
                  <td>{a.granicna_vrednostMax}</td>
                  <td>{new Date(a.vreme_dogadjaja).toLocaleString()}</td>
                  <td>{a.komentar}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
