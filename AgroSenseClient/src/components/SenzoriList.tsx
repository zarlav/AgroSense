import { useEffect, useState } from "react";
import "./SenzoriList.css";

export interface SenzorDto {
  senzorId: number;
  lokacijaId: number,
  naziv: string;
  proizvodjac: string;
  model: string;
  status: string;
  vremeInstalacije: string;
}

export default function SenzoriList() {
  const [senzori, setSenzori] = useState<SenzorDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetch("https://localhost:7025/api/Senzor/svi_senzori")
      .then((res) => {
        if (!res.ok) throw new Error("Greška pri učitavanju senzora");
        return res.json();
      })
      .then((data: SenzorDto[]) => setSenzori(data))
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p className="senzori-loading">Učitavanje...</p>;
  if (error) return <p className="senzori-error">{error}</p>;

  return (
    <div className="senzori">
      <div className="senzori-table-wrapper">
        <table className="senzori-table">
          <thead>
            <tr>
              <th>Id senzora</th>
              <th>Naziv</th>
              <th>Proizvođač</th>
              <th>Model</th>
              <th>Status</th>
              <th>Vreme instalacije</th>
            </tr>
          </thead>
          <tbody>
            {senzori.map((s, index) => (
              <tr key={index}>
                <td>{s.senzorId}</td>
                <td>{s.naziv}</td>
                <td>{s.proizvodjac}</td>
                <td>{s.model}</td>
                <td className={`status ${s.status.toLowerCase()}`}>
                  {s.status}
                </td>
                <td>{new Date(s.vremeInstalacije).toLocaleString()}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
