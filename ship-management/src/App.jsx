import { BrowserRouter, Routes, Route, NavLink } from 'react-router-dom';
import Ships from './pages/Ships';
import Ports from './pages/Ports';
import ShipVisits from './pages/ShipVisits';
import Cargoes from './pages/Cargoes';
import CrewMembers from './pages/CrewMembers';
import ShipCrewAssignments from './pages/ShipCrewAssignments';
import Dashboard from './pages/Dashboard';
import './App.css';

function App() {
  return (
    <BrowserRouter>
      <div className="app">
        <nav className="navbar">
          <div className="navbar-brand">🚢 Liman Takip Sistemi</div>
          <div className="navbar-links">
            <NavLink to="/dashboard" className={({ isActive }) => isActive ? 'active' : ''}>Dashboard</NavLink>
            <NavLink to="/ships" className={({ isActive }) => isActive ? 'active' : ''}>Gemiler</NavLink>
            <NavLink to="/ports" className={({ isActive }) => isActive ? 'active' : ''}>Limanlar</NavLink>
            <NavLink to="/visits" className={({ isActive }) => isActive ? 'active' : ''}>Ziyaretler</NavLink>
            <NavLink to="/cargoes" className={({ isActive }) => isActive ? 'active' : ''}>Yükler</NavLink>
            <NavLink to="/crew" className={({ isActive }) => isActive ? 'active' : ''}>Mürettebat</NavLink>
            <NavLink to="/assignments" className={({ isActive }) => isActive ? 'active' : ''}>Atamalar</NavLink>
          </div>
        </nav>
        <main className="main-content">
          <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/dashboard" element={<Dashboard />} />
            <Route path="/ships" element={<Ships />} />
            <Route path="/ports" element={<Ports />} />
            <Route path="/visits" element={<ShipVisits />} />
            <Route path="/cargoes" element={<Cargoes />} />
            <Route path="/crew" element={<CrewMembers />} />
            <Route path="/assignments" element={<ShipCrewAssignments />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}

export default App;