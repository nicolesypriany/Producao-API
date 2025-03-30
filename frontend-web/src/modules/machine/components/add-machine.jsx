import RegisterModal from "@/modules/core/components/modal";
import { Flex, TextInput } from "@mantine/core";
import { useForm } from "@mantine/form";
import { createMachine as createMachineService } from "../services";

export const AddMachine = () => {
  const { key, getInputProps, onSubmit } = useForm({
    mode: "uncontrolled",
    initialValues: {
      machineName: "",
      brand: "",
    },
  });

  return (
    <RegisterModal title={"Cadastrar máquina"} registerType="máquinas">
      <form
        onSubmit={onSubmit(async (data) => await createMachineService(data))}
      >
        <Flex direction="column" gap={4}>
          <TextInput
            label="Nome da máquina"
            placeholder="Digite o nome da máquina"
            key={key("machineName")}
            {...getInputProps("machineName")}
          />

          <TextInput
            label="Marca"
            placeholder="Digite o nome da marca"
            key={key("brand")}
            {...getInputProps("brand")}
          />
        </Flex>
      </form>
    </RegisterModal>
  );
};
