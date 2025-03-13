import React, { useState } from 'react';
import './App.css';
import LiveData from './LiveData';  // Komponenta pro zobrazení LiveData
import SavedData from './SavedData';  // Komponenta pro zobrazení SavedData

function App() {
  const [activeTab, setActiveTab] = useState('liveData');

  return (
    <div className="App">
      <h1>Welcome to Bitcoin Price Tracker</h1>
      
      <div className="tabs">
        <button
          className={activeTab === 'liveData' ? 'active' : ''}
          onClick={() => setActiveTab('liveData')}
        >
          Live Data
        </button>
        <button
          className={activeTab === 'savedData' ? 'active' : ''}
          onClick={() => setActiveTab('savedData')}
        >
          Saved Data
        </button>
      </div>

      <div className="tab-content">
        {activeTab === 'liveData' && <LiveData />}  {/* Zobrazení LiveData */}
        {activeTab === 'savedData' && <SavedData />}  {/* Zobrazení SavedData */}
      </div>
    </div>
  );
}

export default App;
