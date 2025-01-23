import { useState } from "react";
import { useForm } from "react-hook-form";
import {
  editMachine as editMachineService,
  getMachine as getMachineService,
  removeMachine as removeMachineService,
} from "../services";

export const useGetMachines = () => {
  const [machines, setMachines] = useState([]);
  const [isEditingMachine, setIsEditingMachine] = useState(null);

  const { register, handleSubmit, reset } = useForm();

  const handleGetMachines = async () => {
    const machines = await getMachineService();
    setMachines(machines);
  };

  const handleEditMachine = async (idMachine) => setIsEditingMachine(idMachine);
  const handleCancelEditMachine = async () => {
    setIsEditingMachine(null);
    reset();
  };

  const handleSaveEditMachine = async (idMachine, data) => {
    try {
      const result = await editMachineService(idMachine, {
        nome: data.machineName,
        marca: data.markName,
      });
      console.log({ idMachine, data, result });
      setMachines((prevMachines) =>
        prevMachines.map((machine) =>
          machine.id === idMachine
            ? { ...machine, nome: data.machineName, marca: data.markName }
            : machine
        )
      );
      setIsEditingMachine(null);
    } catch (error) {
      console.error("Error:", error);
    }
  };

  const handleRemoveMachine = async (idMachine) => {
    await removeMachineService(idMachine);
    setMachines((prevMachines) =>
      prevMachines.filter((machine) => machine.id !== idMachine)
    );
  };

  return {
    isEditingMachine,
    machines,
    handleCancelEditMachine,
    handleEditMachine,
    handleGetMachines,
    handleRemoveMachine,
    handleSaveEditMachine,
    handleSubmit,
    register,
  };
};
