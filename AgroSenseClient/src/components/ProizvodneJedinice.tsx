import { useState } from "react";
import "./ProizvodneJedinice.css";

export default function ProizvodneJedinice() {
    const [lista, setLista] = useState<any[]>([]);
    const [loading, setLoading] = useState(false);
    
    // Inicijalno stanje sa SVIM poljima iz backend DTO-a
    const [forma, setForma] = useState({
        tip_jedinice: "", naziv: "", geo_lat: 0, geo_long: 0, povrsina: 0,
        nadmorska_visina: 0, Vrsta_biljaka: "", status_mreze: "", Odgovorno_lice: "", opis: "",
        aktivno: true,
        granica_temp_min: 0, granica_temp_max: 0,
        granica_vlaznost_min: 0, granica_vlaznost_max: 0,
        granica_vlaznost_lista_min: 0, granica_vlaznost_lista_max: 0,
        granica_co2_min: 0, granica_co2_max: 0,
        granica_jacina_vetra_min: 0, granica_jacina_vetra_max: 0,
        granica_pritisak_vazduha_min: 0, granica_pritisak_vazduha_max: 0,
        granica_pritisak_u_cevima_min: 0, granica_pritisak_u_cevima_max: 0,
        granica_svetlost_min: 0, granica_svetlost_max: 0
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value, type } = e.target;
        setForma(prev => ({
            ...prev,
            [name]: type === 'number' ? parseFloat(value) : value
        }));
    };

    const ucitajSve = async () => {
    setLoading(true);
    try {
        const res = await fetch("https://localhost:7025/api/ProizvodneJedinice");
        const data = await res.json();
        
        // OVA LINIJA JE KLJUČNA - POGLEDAJ JE U BROUSERU (F12 -> Console)
        console.log("BACKEND POSLAO OVO:", data[0]); 
        
        setLista(data);
    } catch (err) { console.error(err); }
    finally { setLoading(false); }
    };

    const dodajJedinicu = async () => {
        try {
            const res = await fetch("https://localhost:7025/api/ProizvodneJedinice", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ ...forma, datum_postavljanja: new Date().toISOString() })
            });
            if (res.ok) { alert("Uspešno dodata jedinica!"); ucitajSve(); }
        } catch (err) { alert("Greška pri čuvanju!"); }
    };

    return (
        <div className="proizvodne-jedinice-container">
            <div className="forma-grid">
                <div className="sekcija-naslov">Osnovni podaci i Lokacija</div>
                <input name="naziv" className="merenjePoDanuInput" placeholder="Naziv" onChange={handleChange} />
                <input name="tip_jedinice" className="merenjePoDanuInput" placeholder="Tip (npr. Staklenik A)" onChange={handleChange} />
                <input name="Odgovorno_lice" className="merenjePoDanuInput" placeholder="Odgovorno lice" onChange={handleChange} />
                <input name="Vrsta_biljaka" className="merenjePoDanuInput" placeholder="Vrsta biljaka" onChange={handleChange} />
                <input name="povrsina" type="number" className="merenjePoDanuInput" placeholder="Površina (m²)" onChange={handleChange} />
                <input name="nadmorska_visina" type="number" className="merenjePoDanuInput" placeholder="Nadmorska visina (m)" onChange={handleChange} />
                <input name="geo_lat" type="number" className="merenjePoDanuInput" placeholder="Geografska širina" onChange={handleChange} />
                <input name="geo_long" type="number" className="merenjePoDanuInput" placeholder="Geografska dužina" onChange={handleChange} />
                <input name="status_mreze" className="merenjePoDanuInput" placeholder="Status mreže" onChange={handleChange} />
                <input name="opis" className="merenjePoDanuInput" style={{ gridColumn: 'span 2' }} placeholder="Kratak opis jedinice" onChange={handleChange} />

                <div className="sekcija-naslov">Atmosferski parametri (Min / Max)</div>
                <div className="input-group-row">
                    <input name="granica_temp_min" type="number" className="merenjePoDanuInput" placeholder="Temp Min" onChange={handleChange} />
                    <input name="granica_temp_max" type="number" className="merenjePoDanuInput" placeholder="Temp Max" onChange={handleChange} />
                </div>
                <div className="input-group-row">
                    <input name="granica_vlaznost_min" type="number" className="merenjePoDanuInput" placeholder="Vlažnost Min" onChange={handleChange} />
                    <input name="granica_vlaznost_max" type="number" className="merenjePoDanuInput" placeholder="Vlažnost Max" onChange={handleChange} />
                </div>
                <div className="input-group-row">
                    <input name="granica_co2_min" type="number" className="merenjePoDanuInput" placeholder="CO₂ Min" onChange={handleChange} />
                    <input name="granica_co2_max" type="number" className="merenjePoDanuInput" placeholder="CO₂ Max" onChange={handleChange} />
                </div>
                <div className="input-group-row">
                    <input name="granica_pritisak_vazduha_min" type="number" className="merenjePoDanuInput" placeholder="Pritisak vazd. Min" onChange={handleChange} />
                    <input name="granica_pritisak_vazduha_max" type="number" className="merenjePoDanuInput" placeholder="Pritisak vazd. Max" onChange={handleChange} />
                </div>

                <div className="sekcija-naslov">Vetar, Svetlost i Navodnjavanje</div>
                <div className="input-group-row">
                    <input name="granica_jacina_vetra_min" type="number" className="merenjePoDanuInput" placeholder="Vetra Min" onChange={handleChange} />
                    <input name="granica_jacina_vetra_max" type="number" className="merenjePoDanuInput" placeholder="Vetra Max" onChange={handleChange} />
                </div>
                <div className="input-group-row">
                    <input name="granica_svetlost_min" type="number" className="merenjePoDanuInput" placeholder="Svetlost Min" onChange={handleChange} />
                    <input name="granica_svetlost_max" type="number" className="merenjePoDanuInput" placeholder="Svetlost Max" onChange={handleChange} />
                </div>
                <div className="input-group-row">
                    <input name="granica_vlaznost_lista_min" type="number" className="merenjePoDanuInput" placeholder="Vl. lista Min" onChange={handleChange} />
                    <input name="granica_vlaznost_lista_max" type="number" className="merenjePoDanuInput" placeholder="Vl. lista Max" onChange={handleChange} />
                </div>
                <div className="input-group-row">
                    <input name="granica_pritisak_u_cevima_min" type="number" className="merenjePoDanuInput" placeholder="Prit. cevi Min" onChange={handleChange} />
                    <input name="granica_pritisak_u_cevima_max" type="number" className="merenjePoDanuInput" placeholder="Prit. cevi Max" onChange={handleChange} />
                </div>

                <div className="forma-actions">
                    <button className="merenjaPoDanu-btn" onClick={dodajJedinicu}>Dodaj jedinicu</button>
                    <button className="merenjaPoDanu-btn btn-prikazi" onClick={ucitajSve}>Prikaži sve (Sortirano)</button>
                </div>
            </div>

            {loading ? <p className="merenja-loading">Učitavanje...</p> : (
                <div className="merenja-table-wrapper">
                    <table className="merenja-table">
                        <thead>
                            <tr>
                                <th>Tip</th>
                                <th>Naziv</th>
                                <th>Površina</th>
                                <th>Biljke</th>
                                <th>Odgovorno lice</th>
                                <th>Status</th>
                            </tr>
                        </thead>
                        <tbody>
                            {lista.map((j, i) => (
                                <tr key={i}>
                                    <td><span className="tabela-tip-oznaka">{j.tip_jedinice}</span></td>
                                    <td style={{ fontWeight: '600' }}>{j.naziv}</td>
                                    <td>{j.povrsina} m²</td>
                                    <td>{j.vrsta_biljaka}</td>
                                    <td>{j.odgovorno_lice}</td>
                                    <td>{j.aktivno ? "✅ Aktivan" : "❌ Inaktivan"}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}