import React, { useState, useEffect } from 'react';
import axios from 'axios';

const SavedData = () => {
  const [savedData, setSavedData] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchSavedData = async () => {
      try {
        const response = await axios.get("https://localhost:7081/api/BitcoinData");
        console.log("Fetched saved data:", response.data);
        setSavedData(response.data);
      } catch (error) {
        console.error("Error fetching saved data:", error);
      }
      setLoading(false);
    };

    fetchSavedData();
  }, []);

  // Funkce pro změnu hodnoty Note
  const handleNoteChange = (index, event) => {
    const newSavedData = [...savedData];
    newSavedData[index].note = event.target.value; // Aktualizace poznámky pro příslušný záznam
    setSavedData(newSavedData);
  };

  // Funkce pro uložení změněných dat pro všechny záznamy
  const handleSaveChanges = async () => {
    try {
      for (const data of savedData) {
        await axios.put(`https://localhost:7081/api/BitcoinData/${data.id}`, data); 
      }
      alert("Changes saved successfully!");
    } catch (error) {
      console.error("Error saving changes:", error);
    }
  };

  // Funkce pro uložení vybraných záznamů
  const handleSaveSelected = async () => {
    const selectedData = savedData.filter(data => data.isSelected); 
    if (selectedData.length === 0) {
      alert("No data selected for saving.");
      return;
    }

    try {
      for (const data of selectedData) {
        await axios.put(`https://localhost:7081/api/BitcoinData/${data.id}`, data); 
      }
      alert("Selected data saved successfully!");
    } catch (error) {
      console.error("Error saving selected data:", error);
    }
  };

  // Funkce pro smazání vybraných záznamů
  const handleDeleteSelected = async () => {
    const selectedIds = savedData.filter(data => data.isSelected).map(data => data.id); 
    try {
      for (const id of selectedIds) {
        await axios.delete(`https://localhost:7081/api/BitcoinData/${id}`);
      }
      // Po smazání, načti data znovu
      const response = await axios.get("https://localhost:7081/api/BitcoinData");
      setSavedData(response.data);
      alert("Selected records deleted!");
    } catch (error) {
      console.error("Error deleting selected data:", error);
    }
  };

  // Funkce pro změnu stavu zaškrtnutí záznamu
  const handleSelectChange = (index) => {
    const newSavedData = [...savedData];
    newSavedData[index].isSelected = !newSavedData[index].isSelected;
    setSavedData(newSavedData);
  };

  return (
    <div>
      <h2>Saved Bitcoin Data</h2>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <div>
          <table>
            <thead>
              <tr>
                <th>Select</th>
                <th>Price (EUR)</th>
                <th>Price (CZK)</th>
                <th>Timestamp</th>
                <th>Note</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {savedData.map((data, index) => (
                <tr key={data.id}>
                  <td>
                    <input
                      type="checkbox"
                      checked={data.isSelected || false}
                      onChange={() => handleSelectChange(index)}
                    />
                  </td>
                  <td>{data.priceEUR}</td>
                  <td>{data.priceCZK}</td>
                  <td>{new Date(data.timestamp).toLocaleString()}</td>
                  <td>
                    <input
                      type="text"
                      value={data.note}
                      onChange={(e) => handleNoteChange(index, e)} 
                    />
                  </td>
                  <td>
                    {/* Tlačítko pro uložení změněné poznámky pro jednotlivý záznam */}
                    <button onClick={handleSaveChanges}>Save Changes</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <button onClick={handleDeleteSelected}>Delete Selected</button>
          <button onClick={handleSaveSelected}>Save Selected</button> {/* Nové tlačítko pro ukládání vybraných dat */}
        </div>
      )}
    </div>
  );
};

export default SavedData;
