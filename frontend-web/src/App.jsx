import "@mantine/core/styles.css";
import { Route, Routes } from "react-router";
import RootLayout from "./layouts/root";
import Home from "./pages/home";
import Machine from "./pages/machine";
import RawMaterial from "./pages/raw-material";
import { pathRoutes } from "./routers";

export default function App() {
  return (
    <Routes>
      <Route element={<RootLayout />}>
        <Route index element={<Home />} />
        <Route path={pathRoutes.machines} element={<Machine />} />
        <Route path={pathRoutes.rawMaterial} element={<RawMaterial />} />
      </Route>
    </Routes>
  );
}
