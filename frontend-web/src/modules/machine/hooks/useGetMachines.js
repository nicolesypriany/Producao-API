import { useState } from "react";
import { getMachine as getMachineService } from "../services";

export function useGetMachines() {
  const [machines, setMachines] = useState([]);

  const handleGetMachines = async () => {
    const machines = await getMachineService();
    setMachines(machines);
  };

  return {
    machines,
    handleGetMachines,
  };
}
