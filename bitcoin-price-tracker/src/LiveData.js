import React, { useState, useEffect, useRef } from 'react';
import axios from 'axios';
import { Line } from 'react-chartjs-2';  // Import pro Line graf
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend } from 'chart.js';

// Registrování komponent pro Chart.js
ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
);

const LiveData = () => {
    const [bitcoinData, setBitcoinData] = useState([]);  // Uložení dat jako pole
    const [loading, setLoading] = useState(true);
    const [selectedRows, setSelectedRows] = useState([]); // Ukládání vybraných řádků
    const hasFetchedData = useRef(false);  // Ref pro kontrolu, jestli už byla data načtena
    const [chartData, setChartData] = useState({}); // Uložení dat pro graf

    useEffect(() => {
        const fetchBitcoinData = async () => {
            try {
                console.log("Calling API...");
                const response = await axios.get("https://localhost:7081/api/Bitcoin/price");
                console.log("API Response Data:", response.data);

                if (response.data.bitcoinPriceEUR && response.data.bitcoinPriceCZK) {
                    setBitcoinData((prevData) => [
                        ...prevData.filter(item => item.timestamp !== new Date().toISOString()), // Kontrola duplicity
                        {
                            id: Date.now(), 
                            priceEUR: response.data.bitcoinPriceEUR,
                            priceCZK: response.data.bitcoinPriceCZK,
                            timestamp: new Date().toISOString(), 
                        },
                    ]);
                } else {
                    console.error("Invalid data from API:", response.data);
                }
            } catch (error) {
                console.error("Error fetching Bitcoin data:", error);
            }
            setLoading(false);  // Po načtení dat už není loading
        };

        // Zkontrolujeme, zda už byla data načtena, aby nedocházelo k opakovanému volání API
        if (!hasFetchedData.current) {
            hasFetchedData.current = true;
            fetchBitcoinData();  // Voláme API pouze jednou při mountování
        }

        // Interval pro pravidelný update každou minutu
        const interval = setInterval(fetchBitcoinData, 60000); 
        return () => {
            clearInterval(interval); 
        };
    }, []); // useEffect s prázdnými závislostmi - volání pouze při mountu komponenty

    // Funkce pro ukládání vybraných dat
    const handleSaveSelected = async () => {
        try {
            const selectedData = bitcoinData.filter((item) =>
                selectedRows.includes(item.id)
            );
            if (selectedData.length === 0) {
                alert("No data selected for saving.");
                return;
            }

            console.log("Data to be saved:", selectedData);

            // Data posíláme s původními názvy polí: priceEUR a priceCZK
            const dataToSave = selectedData.map((item) => ({
                priceEUR: item.priceEUR,  
                priceCZK: item.priceCZK,  
                timestamp: item.timestamp,
            }));

            // Odesíláme data na server na endpoint /api/BitcoinData/bulk
            await axios.post("https://localhost:7081/api/BitcoinData/bulk", dataToSave);
            alert("Selected data saved successfully!");
        } catch (err) {
            console.error("Error saving selected data:", err);
        }
    };

    // Funkce pro změnu stavu checkboxu
    const handleCheckboxChange = (id) => {
        setSelectedRows((prevSelected) => {
            if (prevSelected.includes(id)) {
                return prevSelected.filter((itemId) => itemId !== id);
            } else {
                return [...prevSelected, id];
            }
        });
    };

    // Generování dat pro graf
    useEffect(() => {
        const labels = bitcoinData.map(item => new Date(item.timestamp).toLocaleString());
        const dataEUR = bitcoinData.map(item => item.priceEUR);

        setChartData({
            labels: labels,
            datasets: [{
                label: 'Price (EUR)',
                data: dataEUR,
                borderColor: 'rgba(75, 192, 192, 1)',
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                fill: true,
            }]
        });
    }, [bitcoinData]);  // Tento useEffect bude spuštěn pokaždé, když se změní bitcoinData

    return (
        <div>
            <h2>Live Bitcoin Data</h2>
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
                            </tr>
                        </thead>
                        <tbody>
                            {bitcoinData.map((data) => (
                                <tr key={data.id}>
                                    <td>
                                        <input
                                            type="checkbox"
                                            checked={selectedRows.includes(data.id)}
                                            onChange={() => handleCheckboxChange(data.id)}
                                        />
                                    </td>
                                    <td>{data.priceEUR}</td>
                                    <td>{data.priceCZK}</td>
                                    <td>{new Date(data.timestamp).toLocaleString()}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    <button onClick={handleSaveSelected}>Save Selected</button>
                    
                    {/* Graf pro zobrazení vývoje ceny */}
                    <div style={{ marginTop: '30px', width: '100%', height: '400px' }}>
                        <Line data={chartData} />
                    </div>
                </div>
            )}
        </div>
    );
};

export default LiveData;
