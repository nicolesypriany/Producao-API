import { Route, Routes } from "react-router";
import PublicLayout from "./layouts/root";
import Home from "./pages/home";
import Create from "./pages/machine/create";
import { routes } from "./routers";

export default function App() {
  return (
    <Routes>
      <Route element={<PublicLayout />}>
        <Route index element={<Home />} />
        <Route path={routes.createMachines} element={<Create />} />
      </Route>
    </Routes>
  );
}
