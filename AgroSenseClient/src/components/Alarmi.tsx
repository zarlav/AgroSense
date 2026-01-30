import { useState } from "react";
import "./Alarmi.css";

interface Alarm {
  tipSenzora: string;
  parametar: string;
  trenutnaVrednost: number;
  granicnaVrednost: number;
  stanjePre: string;
  stanjePosle: string;
  prioritet: string;
  vremeAktivacije: string;
  komentar: string;
}

export default function Alarmi() {
  const [lokacijaId, setLokacijaId] = useState("");
  const [dan, setDan] = useState("");
  const [alarmi, setAlarmi] = useState<Alarm[]>([]);
  const [loading, setLoading] = useState(false);

  const prikaziAlarme = async () => {
    if (!lokacijaId || !dan) {
      alert("Unesi parcelu (lokacijaId) i datum");
      return;
    }

    try {
      setLoading(true);
      const response = await fetch(
        `http://localhost:5161/api/alarm/po_lokaciji?lokacijaId=${lokacijaId}&dan=${dan}`
      );

      if (!response.ok) {
        setAlarmi([]);
        alert("Nema alarma za izabranu parcelu ili greška u API-ju");
        return;
      }

      const data = await response.json();
      // safety: ako polja nedostaju, dodeli default vrednosti
      const safeData: Alarm[] = data.map((a: any) => ({
        tipSenzora: a.tipSenzora ?? "",
        parametar: a.parametar ?? "",
        trenutnaVrednost: a.trenutnaVrednost ?? 0,
        granicnaVrednost: a.granicnaVrednost ?? 0,
        stanjePre: a.stanjePre ?? "",
        stanjePosle: a.stanjePosle ?? "",
        prioritet: a.prioritet ?? "",
        vremeAktivacije: a.vremeAktivacije ?? "",
        komentar: a.komentar ?? ""
      }));

      setAlarmi(safeData);
    } catch (err) {
      console.error(err);
      alert("Greška pri učitavanju alarma");
      setAlarmi([]);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="alarmi">
      <h2>Alarmi po parceli</h2>

      <div className="alarmi-Forma">
        <div className="alarmi-Polje">
          <label>Lokacija ID (parcela)</label>
          <input
            type="text"
            className="alarmiInput"
            placeholder="Unesi ID lokacije"
            value={lokacijaId}
            onChange={(e) => setLokacijaId(e.target.value)}
          />
        </div>

        <div className="alarmi-Polje">
          <label>Datum</label>
          <input
            type="date"
            className="alarmiInput"
            value={dan}
            onChange={(e) => setDan(e.target.value)}
          />
        </div>

        <button className="alarmi-btn" onClick={prikaziAlarme}>
          Prikaži alarme
        </button>
      </div>

      {loading && <p className="alarmi-loading">Učitavanje...</p>}

      {!loading && alarmi.length === 0 && <p className="alarmi-error">Nema alarma za izabranu parcelu.</p>}

      {alarmi.length > 0 && (
        <div className="alarmi-table-wrapper">
          <table className="alarmi-table">
            <thead>
              <tr>
                <th>Tip senzora</th>
                <th>Parametar</th>
                <th>Trenutna</th>
                <th>Granica</th>
                <th>Pre</th>
                <th>Posle</th>
                <th>Prioritet</th>
                <th>Vreme</th>
                <th>Komentar</th>
              </tr>
            </thead>
            <tbody>
              {alarmi.map((a, i) => (
                <tr key={i}>
                  <td>{a.tipSenzora}</td>
                  <td>{a.parametar}</td>
                  <td>{a.trenutnaVrednost}</td>
                  <td>{a.granicnaVrednost}</td>
                  <td>{a.stanjePre}</td>
                  <td>{a.stanjePosle}</td>
                  <td>{a.prioritet}</td>
                  <td>{a.vremeAktivacije ? new Date(a.vremeAktivacije).toLocaleString() : ""}</td>
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
