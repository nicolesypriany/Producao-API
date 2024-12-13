import { Route, Routes } from "react-router";
import PublicLayout from "./layouts/root";
import Home from "./pages/home";
import Machine from "./pages/machine";

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<PublicLayout />}>
        <Route index element={<Home />} />
        <Route path="/machine" element={<Machine />} />
      </Route>
    </Routes>
  );
}
