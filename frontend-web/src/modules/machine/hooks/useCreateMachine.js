import { createMachine as createMachineService } from "@/machine/services";
import { useForm } from "react-hook-form";

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
