const express = require('express');
const axios = require('axios');

const app = express();
const PORT = process.env.PORT || 3000;

// Replace with your actual Render API token
const RENDER_API_TOKEN = "rnd_9FUqI6yrSN5LACpI1k4ol7k5DRhb";

// Endpoint to get installed applications
app.get('/', async (req, res) => {
    console.log('Fetching installed applications...');
    console.log(RENDER_API_TOKEN);
    
    try {        
        const response = await axios.get('https://api.render.com/v1/services', {
            headers: {
                'Authorization': `Bearer ${RENDER_API_TOKEN}`,
                'Accept': 'application/json'
            }
        });
        res.json(response.data);
    } catch (error) {
        console.error(error);
        res.status(500).json({ message: 'Error fetching applications' });
    }
});

app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});
//rnd_9FUqI6yrSN5LACpI1k4ol7k5DRhb
