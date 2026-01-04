import { useState } from "react";
import SenzoriList from "./SenzoriList";
import MerenjaList from "./MerenjaList";
import MerenjaPoVremenu from "./MerenjaPoVremenu";
import MerenjaPoLokaciji from "./MerenjaPoLokaciji";
import "./Kartice.css";

export default function Kartice() {
  const [activeTab, setActiveTab] = useState("Poslednje merenje");

  const renderContent = () => {
    switch (activeTab) {
      case "Senzori":
        return <SenzoriList />;
      case "Poslednje merenje":
        return <MerenjaList />;
      case "Merenja po vremenu":
        return <MerenjaPoVremenu />;
      case "Merenja po lokaciji":
        return <MerenjaPoLokaciji />;
      default:
        return null;
    }
  };

  return (
    <div className="kartice">
      <div className="tab-nav">
        {["Senzori", "Poslednje merenje", "Merenja po vremenu", "Merenja po lokaciji"].map((tab) => (
          <button
            key={tab}
            onClick={() => setActiveTab(tab)}
            className={`tab-btn ${activeTab === tab ? "active" : ""}`}
          >
            {tab}
          </button>
        ))}
      </div>

      <div className="tab-content">
        {renderContent()}
      </div>
    </div>
  );
}
