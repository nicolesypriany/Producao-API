import { useState } from "react";
import { useForm } from "react-hook-form";
import {
  editMachine as editMachineService,
  getMachine as getMachineService,
  removeMachine as removeMachineService,
} from "../services";

export const useFormMachines = () => {
  const [machines, setMachines] = useState([]);
  const [editingMachineId, setEditingMachineId] = useState(null);

  const { register, handleSubmit, reset } = useForm();

  const handleEditMachine = async (idMachine) => setEditingMachineId(idMachine);
  const handleCancelEditMachine = async () => {
    setEditingMachineId(null);
    reset();
  };

  const handleSaveEditMachine = async (idMachine, data) => {
    try {
      await editMachineService(idMachine, {
        nome: data.machineName,
        marca: data.markName,
      });
      setMachines((prevMachines) =>
        prevMachines.map((machine) =>
          machine.id === idMachine
            ? { ...machine, nome: data.machineName, marca: data.markName }
            : machine
        )
      );
      setEditingMachineId(null);
    } catch (error) {
      console.error("Error:", error);
    }
  };

  const handleRemoveMachine = async (idMachine) => {
    const response = await removeMachineService(idMachine);
    console.log({ response });
    setMachines((prevMachines) =>
      prevMachines.filter((machine) => machine.id !== idMachine)
    );
  };

  return {
    editingMachineId,
    register,
    handleSubmit,
    handleEditMachine,
    handleRemoveMachine,
    handleSaveEditMachine,
    handleCancelEditMachine,
  };
};
