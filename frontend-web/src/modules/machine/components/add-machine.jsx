import { Button, Flex, Group, Modal, TextInput } from "@mantine/core";

import { useForm } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";

export const ModalAddItem = () => {
  const [opened, { open, close }] = useDisclosure(false);

  const form = useForm({
    mode: "uncontrolled",
    initialValues: {
      machineName: "",
      brand: "",
    },
  });

  return (
    <>
      <Modal opened={opened} onClose={close} title="Cadastrar máquina" centered>
        <form onSubmit={form.onSubmit((values) => console.table(values))}>
          <Flex direction="column" gap={4}>
            <TextInput
              label="Nome da máquina"
              placeholder="Digite o nome da máquina"
              key={form.key("machineName")}
              {...form.getInputProps("machineName")}
            />

            <TextInput
              label="Marca"
              placeholder="Digite o nome da marca"
              key={form.key("brand")}
              {...form.getInputProps("brand")}
            />

            <Group mt="lg" justify="flex-end">
              <Button variant="filled" w="30%">
                Salvar
              </Button>
            </Group>
          </Flex>
        </form>
      </Modal>

      <Button variant="filled" w="30%" size="sm" onClick={open}>
        Cadastrar máquina
      </Button>
    </>
  );
};
