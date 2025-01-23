import { useForm } from "react-hook-form";
import { createMachine as createMachineService } from "../services";

export function useCreateMachine() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({ defaultValues: { machineName: "", mark: "" } });

  const onSubmit = async (data) => {
    console.log(data);
    await createMachineService(data);
  };

  return {
    register,
    handleSubmit,
    errors,
    onSubmit,
  };
}
